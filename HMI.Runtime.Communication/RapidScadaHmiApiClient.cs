// SPDX-License-Identifier: GPL-3.0-only
using Scada;
using Scada.Client;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Protocol;
using System.Globalization;
using HmiLanguage = HMI.Runtime.Communication.Language;

namespace HMI.Runtime.Communication;

public sealed class RapidScadaHmiApiClient : IHmiApiClient, IUnacknowledgedEventSource, IAlarmHistorySource, IAlarmMetadataSource, IHistoricalTrendSource, IOperationEventSource, ISignalCatalogSource
{
    private readonly HmiApiConnectionOptions _options;
    private readonly ConnectionOptions _connectionOptions;
    private readonly ScadaClientPool _clientPool = new();
    private readonly SemaphoreSlim _operationLock = new(1, 1);
    private readonly CancellationTokenSource _disposeTokenSource = new();
    private readonly HashSet<string> _subscriptions = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, CnlData> _lastValues = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Guid, Scada.Data.Models.Event> _alarmEvents = new();
    private ScadaClient? _client;
    private ConfigDataset? _configDataset;
    private Dictionary<string, Cnl>? _channelsByName;
    private Dictionary<int, Scada.Data.Entities.Alarm>? _alarmsById;
    private Dictionary<int, string>? _alarmGroupNamesById;
    private Dictionary<int, string>? _alarmTypeNamesById;
    private Task? _pollTask;
    private UserInfo? _currentUser;
    private int _currentUserId;
    private int _disposed;

    public event Action<UserInfo>? OnUserContextChanged;
    public event Action<SignalValue>? SignalReceived;
    public event Action<string, Exception, int>? CommunicationError;
    public event Action<string, Exception, int, int[], string[]>? RegisterError;

    public RapidScadaHmiApiClient(HmiApiConnectionOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrWhiteSpace(options.Name))
        {
            throw new ArgumentException("A connection name is required.", nameof(options));
        }
        if (string.IsNullOrWhiteSpace(options.Host) || options.Port is <= 0 or > 65535)
        {
            throw new ArgumentException("A valid RapidSCADA host and port are required.", nameof(options));
        }
        if (options.SubscriptionPollingIntervalMilliseconds is < 100 or > 60000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "The subscription polling interval must be between 100 and 60000 milliseconds.");
        }

        _connectionOptions = new ConnectionOptions
        {
            Name = options.Name,
            Host = options.Host,
            Port = options.Port,
            Username = options.Username,
            Password = ScadaUtils.Decrypt(options.Password),
            Instance = options.Instance,
            Timeout = options.Timeout,
            SecretKey = string.IsNullOrWhiteSpace(options.SecretKey)
                ? Array.Empty<byte>()
                : ScadaUtils.HexToBytes(options.SecretKey)
        };
    }

    public Task AuthenticateAsync(string token, CancellationToken ct = default) =>
        Task.FromException(new NotSupportedException("RapidSCADA connections do not use bearer-token authentication."));

    public async Task<UserInfo> LoginAsync(string user, string password, CancellationToken ct = default)
    {
        UserInfo userInfo = await WithClientAsync(client =>
        {
            UserValidationResult result = client.ValidateUser(user, password);
            if (!result.Handled || !result.IsValid)
            {
                throw new UnauthorizedAccessException(
                    string.IsNullOrWhiteSpace(result.ErrorMessage)
                        ? "RapidSCADA rejected the user credentials."
                        : result.ErrorMessage);
            }

            _currentUserId = result.UserID;
            return new UserInfo
            {
                UserName = user,
                UserLevel = result.RoleID,
                IsDomainUser = false
            };
        }, ct).ConfigureAwait(false);

        _currentUser = userInfo;
        OnUserContextChanged?.Invoke(userInfo);
        return userInfo;
    }

    public Task LogoutAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        _currentUser = null;
        _currentUserId = 0;
        return Task.CompletedTask;
    }

    public Task<object?> ChangePasswordAsUserAsync(
        string username,
        string oldPassword,
        string newPassword,
        CancellationToken ct = default) =>
        Task.FromException<object?>(new NotSupportedException(
            "The RapidSCADA ScadaClient API does not expose a change-password operation."));

    public Task<object?> ChangePasswordAsAdminAsync(
        string username,
        string newPassword,
        CancellationToken ct = default) =>
        Task.FromException<object?>(new NotSupportedException(
            "The RapidSCADA ScadaClient API does not expose a change-password operation."));

    public async Task<int> GetSecurtityStatusAsync(CancellationToken ct = default)
    {
        return await WithClientAsync(client =>
        {
            client.GetStatus(out bool serverIsReady, out _);
            return serverIsReady
                ? (_currentUser is null ? (int)SecurityStatus.SecurityOff : (int)SecurityStatus.SecurityOn)
                : (int)SecurityStatus.SecurityUnknown;
        }, ct).ConfigureAwait(false);
    }

    public Task<object?> ReadSignalAsync(string signal, CancellationToken ct = default)
    {
        return WithClientAsync<object?>(client =>
        {
            Cnl channel = GetChannel(client, signal);
            CnlData[] data = client.GetCurrentData([channel.CnlNum], useCache: true, out _);
            return data[0].IsDefined ? data[0].Val : null;
        }, ct);
    }

    public Task<object?> WriteSignalAsync(
        string signal,
        object value,
        WriteOptions? options = null,
        CancellationToken ct = default)
    {
        return WithClientAsync<object?>(client =>
        {
            ApplyWriteOptions(client, options);
            Cnl channel = GetChannel(client, signal);
            double numericValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            Slice slice = new(
                DateTime.UtcNow,
                [channel.CnlNum],
                [new CnlData(numericValue)]);
            client.WriteCurrentData(slice, WriteDataFlags.Default);
            _lastValues.Remove(channel.Name);
            return numericValue;
        }, ct);
    }

    public async Task SubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(signals);
        string[] requestedSignals = signals
            .Where(signal => !string.IsNullOrWhiteSpace(signal))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (requestedSignals.Length == 0)
        {
            return;
        }

        await _operationLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            await Task.Run(() =>
            {
                ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
                ScadaClient client = GetClient();
                EnsureChannelTableLoaded(client);

                string[] unknownSignals = requestedSignals
                    .Where(signal => !_channelsByName!.ContainsKey(signal))
                    .ToArray();
                if (unknownSignals.Length > 0)
                {
                    InvalidOperationException exception = new(
                        $"RapidSCADA channels were not found: {string.Join(", ", unknownSignals)}");
                    RegisterError?.Invoke(
                        _options.Name,
                        exception,
                        -2,
                        Enumerable.Repeat(-2, unknownSignals.Length).ToArray(),
                        unknownSignals);
                    throw exception;
                }

                foreach (string signal in requestedSignals)
                {
                    _subscriptions.Add(signal);
                }

                PollSignals(client);
            }, ct).ConfigureAwait(false);
            _pollTask ??= Task.Run(() => PollSignalsAsync(_disposeTokenSource.Token));
        }
        finally
        {
            _operationLock.Release();
        }
    }

    public async Task UnsubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(signals);
        string[] requestedSignals = signals
            .Where(signal => !string.IsNullOrWhiteSpace(signal))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        await _operationLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            foreach (string signal in requestedSignals)
            {
                _subscriptions.Remove(signal);
                _lastValues.Remove(signal);
            }
        }
        finally
        {
            _operationLock.Release();
        }
    }

    public Task<IReadOnlyList<UnacknowledgedEvent>> GetUnacknowledgedEventsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("The event query start time must precede its end time.", nameof(startTime));
        }

        return WithClientAsync<IReadOnlyList<UnacknowledgedEvent>>(client =>
        {
            List<Scada.Data.Models.Event> events = client.GetEvents(
                0,
                new TimeRange(startTime.UtcDateTime, endTime.UtcDateTime, endInclusive: true),
                new DataFilter(typeof(Scada.Data.Models.Event)),
                useCache: true,
                out _);

            return events.Where(item => item.AckRequired && !item.Ack)
                .Select(item => new UnacknowledgedEvent(
                    ToAlarmId(item.EventID),
                    GetChannelNameByNumber(client, item.CnlNum),
                    item.Text,
                    item.Severity,
                    item.Timestamp))
                .ToArray();
        }, cancellationToken);
    }

    public Task<IReadOnlyList<AlarmHistoryEvent>> GetAlarmHistoryAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("The event query start time must precede its end time.", nameof(startTime));
        }

        return WithClientAsync<IReadOnlyList<AlarmHistoryEvent>>(client =>
        {
            EnsureAlarmTablesLoaded(client);
            List<Scada.Data.Models.Event> events = client.GetEvents(
                0,
                new TimeRange(startTime.UtcDateTime, endTime.UtcDateTime, endInclusive: true),
                new DataFilter(typeof(Scada.Data.Models.Event)),
                useCache: true,
                out _);

            return events.Where(item => item.AckRequired)
                .Select(item => new AlarmHistoryEvent(
                    ToAlarmId(item.EventID),
                    GetChannelNameByNumber(client, item.CnlNum),
                    item.Text,
                    item.Severity,
                    item.Timestamp,
                    item.Ack,
                    item.AckTimestamp == default ? null : item.AckTimestamp,
                    GetAlarmGroupName(item.AlarmID),
                    GetAlarmTypeName(item.AlarmID)))
                .OrderByDescending(item => item.Timestamp)
                .ToArray();
        }, cancellationToken);
    }

    public Task<IReadOnlyList<OperationEvent>> GetOperationEventsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int maxEvents,
        CancellationToken cancellationToken = default)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("The event query start time must precede its end time.", nameof(startTime));
        }
        if (maxEvents is < 1 or > 10000)
        {
            throw new ArgumentOutOfRangeException(nameof(maxEvents), "The event limit must be between 1 and 10000.");
        }

        return WithClientAsync<IReadOnlyList<OperationEvent>>(client =>
        {
            List<Scada.Data.Models.Event> events = client.GetEvents(
                0,
                new TimeRange(startTime.UtcDateTime, endTime.UtcDateTime, endInclusive: true),
                new DataFilter(typeof(Scada.Data.Models.Event)),
                useCache: true,
                out _);

            return events
                .OrderByDescending(item => item.Timestamp)
                .Take(maxEvents)
                .Select(item => new OperationEvent(
                    item.EventID,
                    item.Timestamp,
                    GetChannelNameByNumber(client, item.CnlNum),
                    item.Text ?? string.Empty,
                    double.IsFinite(item.PrevCnlVal) ? item.PrevCnlVal : null,
                    double.IsFinite(item.CnlVal) ? item.CnlVal : null,
                    item.Severity,
                    item.AckRequired,
                    item.Ack,
                    item.AckTimestamp == default ? null : item.AckTimestamp,
                    item.DeviceNum,
                    item.ObjNum))
                .ToArray();
        }, cancellationToken);
    }

    public Task<AlarmFilterMetadata> GetAlarmFilterMetadataAsync(CancellationToken cancellationToken = default)
    {
        return WithClientAsync(client =>
        {
            EnsureAlarmTablesLoaded(client);
            return new AlarmFilterMetadata(
                BuildFilterPaths(_alarmGroupNamesById!),
                BuildFilterPaths(_alarmTypeNamesById!));
        }, cancellationToken);
    }

    public Task<IReadOnlyList<HistoricalTrendSeries>> GetHistoricalTrendsAsync(
        IReadOnlyList<string> signalNames,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int archiveBit = 0,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(signalNames);
        if (startTime >= endTime)
        {
            throw new ArgumentException("The trend query start time must precede its end time.", nameof(startTime));
        }

        if (archiveBit is < 0 or > byte.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(archiveBit), "The RapidSCADA archive bit must be between 0 and 255.");
        }

        string[] requestedSignals = signalNames
            .Select(signal => signal?.Trim() ?? string.Empty)
            .Where(signal => signal.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (requestedSignals.Length == 0)
        {
            return Task.FromResult<IReadOnlyList<HistoricalTrendSeries>>([]);
        }

        return WithClientAsync<IReadOnlyList<HistoricalTrendSeries>>(client =>
        {
            EnsureChannelTableLoaded(client);
            TimeRange range = new(startTime.UtcDateTime, endTime.UtcDateTime, endInclusive: true);
            List<HistoricalTrendSeries> trends = new(requestedSignals.Length);
            foreach (string signalName in requestedSignals)
            {
                Cnl channel = GetChannel(client, signalName);
                Scada.Data.Models.Trend trend = client.GetTrend(archiveBit, range, channel.CnlNum);
                trends.Add(new HistoricalTrendSeries(
                    channel.Name,
                    trend.Points
                        .Select(point => new HistoricalTrendPoint(point.Timestamp, point.Val, point.Stat))
                        .ToArray()));
            }
            return trends;
        }, cancellationToken);
    }

    public Task<IReadOnlyList<string>> GetSignalNamesAsync(CancellationToken cancellationToken = default) =>
        WithClientAsync<IReadOnlyList<string>>(client =>
        {
            EnsureChannelTableLoaded(client);
            return _channelsByName!.Keys.OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToArray();
        }, cancellationToken);

    public Task<IEnumerable<Alarm>> GetActiveAlarmsAsync(CancellationToken ct = default)
    {
        return WithClientAsync<IEnumerable<Alarm>>(client =>
        {
            DateTime endTime = DateTime.UtcNow;
            List<Scada.Data.Models.Event> events = client.GetEvents(
                0,
                new TimeRange(endTime.AddDays(-1), endTime, endInclusive: true),
                new DataFilter(typeof(Scada.Data.Models.Event)),
                useCache: true,
                out _);
            _alarmEvents.Clear();

            List<Alarm> alarms = [];
            foreach (Scada.Data.Models.Event scadaEvent in events.Where(item => item.AckRequired && !item.Ack))
            {
                Guid alarmId = ToAlarmId(scadaEvent.EventID);
                _alarmEvents[alarmId] = scadaEvent;
                alarms.Add(new Alarm
                {
                    Id = alarmId,
                    Name = GetChannelNameByNumber(client, scadaEvent.CnlNum),
                    Message = scadaEvent.Text,
                    Severity = scadaEvent.Severity.ToString(CultureInfo.InvariantCulture),
                    Timestamp = scadaEvent.Timestamp,
                    Active = true
                });
            }

            return alarms;
        }, ct);
    }

    public Task<object?> AcknowledgeAlarmsAsync(
        IEnumerable<Guid> alarmIds,
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(alarmIds);
        return WithClientAsync<object?>(client =>
        {
            ApplyWriteOptions(client, options);
            EnsureCommentIsSupported(comment);
            foreach (Guid alarmId in alarmIds.Distinct())
            {
                AcknowledgeEvent(client, alarmId);
            }
            return null;
        }, ct);
    }

    public Task<object?> AcknowledgeAlarmsByGroupAsync(
        string groupName,
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default) =>
        Task.FromException<object?>(new NotSupportedException(
            "RapidSCADA alarm groups are not represented by the IHmiApiClient group-name contract."));

    public Task<object?> AcknowledgeAllAlarmsAsync(
        WriteOptions? options = null,
        CancellationToken ct = default) =>
        AcknowledgeAllAlarmsWithCommentAsync(null, options, ct);

    public Task<object?> AcknowledgeAllAlarmsWithCommentAsync(
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default)
    {
        return WithClientAsync<object?>(client =>
        {
            ApplyWriteOptions(client, options);
            EnsureCommentIsSupported(comment);
            foreach (Guid alarmId in GetActiveAlarmEvents(client).Select(item => ToAlarmId(item.EventID)))
            {
                AcknowledgeEvent(client, alarmId);
            }
            return null;
        }, ct);
    }

    public Task<object?> AcknowledgeAllGoneAlarmsAsync(
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default) =>
        Task.FromException<object?>(new NotSupportedException(
            "RapidSCADA does not provide an equivalent to acknowledging all gone alarms."));

    public Task<object?> SetAlarmStateAsync(
        IEnumerable<string> alarmIds,
        int[] states,
        DateTime[] reactivations,
        WriteOptions? options = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(alarmIds);
        ArgumentNullException.ThrowIfNull(states);
        ArgumentNullException.ThrowIfNull(reactivations);

        string[] ids = alarmIds.ToArray();
        if (ids.Length != states.Length || ids.Length != reactivations.Length)
        {
            throw new ArgumentException("Alarm IDs, states, and reactivation times must have equal lengths.");
        }
        if (reactivations.Any(value => value != default))
        {
            return Task.FromException<object?>(new NotSupportedException(
                "RapidSCADA does not support alarm reactivation times through ScadaClient."));
        }
        if (states.Any(state => state is not 1 and not 3))
        {
            return Task.FromException<object?>(new NotSupportedException(
                "RapidSCADA can acknowledge alarms, but cannot apply the requested alarm state."));
        }

        Guid[] parsedIds = ids.Select(ParseAlarmId).ToArray();
        return AcknowledgeAlarmsAsync(parsedIds, options: options, ct: ct);
    }

    public Task<IEnumerable<HmiLanguage>> GetAllLanguages(CancellationToken ct = default)
    {
        return WithClientAsync<IEnumerable<HmiLanguage>>(client =>
        {
            EnsureTranslationTablesLoaded(client);
            HmiLanguage[] languages = _configDataset!.LanguageTable
                .Select(language => new HmiLanguage
                {
                    LanguageId = language.LanguageID,
                    Description = string.IsNullOrWhiteSpace(language.Descr) ? language.Name : language.Descr,
                    IsActive = true,
                    IsDefault = string.Equals(
                        language.Code,
                        CultureInfo.CurrentUICulture.Name,
                        StringComparison.OrdinalIgnoreCase)
                })
                .ToArray();

            if (languages.Length > 0 && languages.All(language => !language.IsDefault))
            {
                languages[0] = new HmiLanguage
                {
                    LanguageId = languages[0].LanguageId,
                    Description = languages[0].Description,
                    IsActive = true,
                    IsDefault = true
                };
            }

            return languages;
        }, ct);
    }

    public Task<IEnumerable<KeyValuePair<string, string>>> GetSymbolicTextTranslations(
        int languageId,
        CancellationToken ct = default)
    {
        return WithClientAsync<IEnumerable<KeyValuePair<string, string>>>(client =>
        {
            EnsureTranslationTablesLoaded(client);
            return _configDataset!.TranslationTable
                .Where(translation => translation.LanguageID == languageId)
                .Select(translation => new KeyValuePair<string, string>(translation.Code, translation.Text))
                .ToArray();
        }, ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        if (_pollTask is not null)
        {
            await _pollTask.ConfigureAwait(false);
        }

        await _operationLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_client is not null)
            {
                _clientPool.ReturnClient(_client);
                _client = null;
            }
            _subscriptions.Clear();
            _lastValues.Clear();
        }
        finally
        {
            _operationLock.Release();
            _operationLock.Dispose();
            _disposeTokenSource.Dispose();
        }
    }

    private async Task<T> WithClientAsync<T>(Func<ScadaClient, T> operation, CancellationToken ct)
    {
        await _operationLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
            return await Task.Run(() => operation(GetClient()), ct).ConfigureAwait(false);
        }
        finally
        {
            _operationLock.Release();
        }
    }

    private ScadaClient GetClient()
    {
        return _client ??= _clientPool.GetClient(_connectionOptions);
    }

    private void EnsureChannelTableLoaded(ScadaClient client)
    {
        if (_channelsByName is not null)
        {
            return;
        }

        ConfigDataset dataset = _configDataset ??= new ConfigDataset();
        if (!client.DownloadBaseTable(dataset.CnlTable))
        {
            throw new InvalidOperationException($"Failed to download the channel table from '{_options.Name}'.");
        }

        Dictionary<string, Cnl> channels = new(StringComparer.OrdinalIgnoreCase);
        foreach (Cnl channel in dataset.CnlTable)
        {
            if (!channels.TryAdd(channel.Name, channel))
            {
                throw new InvalidOperationException($"RapidSCADA contains duplicate channel name '{channel.Name}'.");
            }
        }
        _channelsByName = channels;
    }

    private void EnsureAlarmTablesLoaded(ScadaClient client)
    {
        if (_alarmsById is not null && _alarmGroupNamesById is not null && _alarmTypeNamesById is not null)
        {
            return;
        }

        ConfigDataset dataset = _configDataset ??= new ConfigDataset();
        if (!client.DownloadBaseTable(dataset.AlarmTable))
        {
            throw new InvalidOperationException($"Failed to download the alarm table from '{_options.Name}'.");
        }
        if (!client.DownloadBaseTable(dataset.AlarmGroupTable))
        {
            throw new InvalidOperationException($"Failed to download the alarm group table from '{_options.Name}'.");
        }
        if (!client.DownloadBaseTable(dataset.AlarmTypeTable))
        {
            throw new InvalidOperationException($"Failed to download the alarm type table from '{_options.Name}'.");
        }

        Dictionary<int, Scada.Data.Entities.Alarm> alarmsById =
            dataset.AlarmTable.ToDictionary(alarm => alarm.AlarmID);
        Dictionary<int, string> groupNamesById = BuildHierarchicalNames(
            dataset.AlarmGroupTable.Select(group => (group.AlarmGroupID, group.Name, group.ParentID)));
        Dictionary<int, string> typeNamesById = BuildHierarchicalNames(
            dataset.AlarmTypeTable.Select(type => (type.AlarmTypeID, type.Name, type.ParentID)));

        _alarmsById = alarmsById;
        _alarmGroupNamesById = groupNamesById;
        _alarmTypeNamesById = typeNamesById;
    }

    private static Dictionary<int, string> BuildHierarchicalNames(
        IEnumerable<(int Id, string Name, int? ParentId)> entities)
    {
        Dictionary<int, (string Name, int? ParentId)> entitiesById = entities.ToDictionary(
            entity => entity.Id,
            entity => (entity.Name, entity.ParentId));
        Dictionary<int, string> namesById = [];
        foreach (int id in entitiesById.Keys)
        {
            List<string> path = [];
            HashSet<int> visited = [];
            int currentId = id;
            while (entitiesById.TryGetValue(currentId, out var current))
            {
                if (!visited.Add(currentId))
                {
                    throw new InvalidOperationException($"Alarm group/type hierarchy contains a cycle at ID '{currentId}'.");
                }
                path.Add(current.Name);
                if (current.ParentId is not int parentId || !entitiesById.ContainsKey(parentId))
                {
                    break;
                }
                currentId = parentId;
            }
            path.Reverse();
            string fullName = string.Join(".", path);
            if (fullName.StartsWith("*.", StringComparison.Ordinal))
            {
                fullName = fullName[2..];
            }
            namesById.Add(id, fullName);
        }
        return namesById;
    }

    private static IReadOnlyList<string> BuildFilterPaths(Dictionary<int, string> namesById) =>
        namesById.Values
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
            .ToArray();

    private string GetAlarmGroupName(int alarmId) =>
        _alarmsById?.TryGetValue(alarmId, out Scada.Data.Entities.Alarm? alarm) == true &&
        alarm.AlarmGroupID is int groupId &&
        _alarmGroupNamesById?.TryGetValue(groupId, out string? groupName) == true
            ? groupName
            : string.Empty;

    private string GetAlarmTypeName(int alarmId) =>
        _alarmsById?.TryGetValue(alarmId, out Scada.Data.Entities.Alarm? alarm) == true &&
        alarm.AlarmTypeID is int typeId &&
        _alarmTypeNamesById?.TryGetValue(typeId, out string? typeName) == true
            ? typeName
            : string.Empty;

    private void EnsureTranslationTablesLoaded(ScadaClient client)
    {
        EnsureChannelTableLoaded(client);
        ConfigDataset dataset = _configDataset!;
        if (!client.DownloadBaseTable(dataset.LanguageTable))
        {
            throw new InvalidOperationException($"Failed to download the language table from '{_options.Name}'.");
        }
        if (!client.DownloadBaseTable(dataset.TranslationTable))
        {
            throw new InvalidOperationException($"Failed to download the translation table from '{_options.Name}'.");
        }
    }

    private Cnl GetChannel(ScadaClient client, string signal)
    {
        if (string.IsNullOrWhiteSpace(signal))
        {
            throw new ArgumentException("A signal name is required.", nameof(signal));
        }

        EnsureChannelTableLoaded(client);
        return _channelsByName!.TryGetValue(signal, out Cnl? channel)
            ? channel
            : throw new KeyNotFoundException($"RapidSCADA channel '{signal}' was not found on '{_options.Name}'.");
    }

    private string GetChannelNameByNumber(ScadaClient client, int channelNumber)
    {
        EnsureChannelTableLoaded(client);
        return _channelsByName!.Values.FirstOrDefault(channel => channel.CnlNum == channelNumber)?.Name
            ?? channelNumber.ToString(CultureInfo.InvariantCulture);
    }

    private async Task PollSignalsAsync(CancellationToken ct)
    {
        using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(_options.SubscriptionPollingIntervalMilliseconds));
        try
        {
            while (await timer.WaitForNextTickAsync(ct).ConfigureAwait(false))
            {
                try
                {
                    await _operationLock.WaitAsync(ct).ConfigureAwait(false);
                    try
                    {
                        if (_subscriptions.Count > 0)
                        {
                            PollSignals(GetClient());
                        }
                    }
                    finally
                    {
                        _operationLock.Release();
                    }
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    CommunicationError?.Invoke(_options.Name, exception, -1);
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
    }

    private void PollSignals(ScadaClient client)
    {
        string[] signalNames = _subscriptions.ToArray();
        if (signalNames.Length == 0)
        {
            return;
        }

        EnsureChannelTableLoaded(client);
        Cnl[] channels = signalNames.Select(signal => GetChannel(client, signal)).ToArray();
        int[] channelNumbers = channels.Select(channel => channel.CnlNum).ToArray();
        CnlData[] values = client.GetCurrentData(channelNumbers, useCache: true, out _);
        if (values.Length != channels.Length)
        {
            throw new InvalidOperationException("RapidSCADA returned a different number of values than requested.");
        }

        for (int index = 0; index < channels.Length; index++)
        {
            Cnl channel = channels[index];
            CnlData value = values[index];
            if (_lastValues.TryGetValue(channel.Name, out CnlData previous) && previous.Equals(value))
            {
                continue;
            }

            _lastValues[channel.Name] = value;
            SignalReceived?.Invoke(new SignalValue
            {
                Name = channel.Name,
                Value = value.IsDefined ? value.Val : null,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    private List<Scada.Data.Models.Event> GetActiveAlarmEvents(ScadaClient client)
    {
        DateTime endTime = DateTime.UtcNow;
        return client.GetEvents(
                0,
                new TimeRange(endTime.AddDays(-1), endTime, endInclusive: true),
                new DataFilter(typeof(Scada.Data.Models.Event)),
                useCache: true,
                out _)
            .Where(item => item.AckRequired && !item.Ack)
            .ToList();
    }

    private void AcknowledgeEvent(ScadaClient client, Guid alarmId)
    {
        long eventId = FromAlarmId(alarmId);
        if (!_alarmEvents.TryGetValue(alarmId, out Scada.Data.Models.Event? scadaEvent))
        {
            scadaEvent = client.GetEventByID(0, eventId);
        }
        if (scadaEvent is null)
        {
            throw new KeyNotFoundException($"RapidSCADA event '{eventId}' was not found.");
        }

        client.AckEvent(new EventAck
        {
            EventID = scadaEvent.EventID,
            Timestamp = scadaEvent.Timestamp,
            UserID = _currentUserId
        });
        _alarmEvents.Remove(alarmId);
    }

    private static void EnsureCommentIsSupported(string? comment)
    {
        if (!string.IsNullOrWhiteSpace(comment))
        {
            throw new NotSupportedException("RapidSCADA ScadaClient does not accept a comment when acknowledging an event.");
        }
    }

    private void ApplyWriteOptions(ScadaClient client, WriteOptions? options)
    {
        if (options is null ||
            (string.IsNullOrWhiteSpace(options.User) && string.IsNullOrWhiteSpace(options.Password)))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(options.User) || string.IsNullOrWhiteSpace(options.Password))
        {
            throw new ArgumentException("Both username and password are required for per-operation authorization.");
        }

        UserValidationResult result = client.ValidateUser(options.User, options.Password);
        if (!result.Handled || !result.IsValid)
        {
            throw new UnauthorizedAccessException(
                string.IsNullOrWhiteSpace(result.ErrorMessage)
                    ? "RapidSCADA rejected the write credentials."
                    : result.ErrorMessage);
        }

        _currentUserId = result.UserID;
        _currentUser = new UserInfo
        {
            UserName = options.User,
            UserLevel = result.RoleID,
            IsDomainUser = false
        };
    }

    private static Guid ToAlarmId(long eventId)
    {
        Span<byte> bytes = stackalloc byte[16];
        BitConverter.TryWriteBytes(bytes, eventId);
        return new Guid(bytes);
    }

    private static Guid ParseAlarmId(string alarmId)
    {
        if (Guid.TryParse(alarmId, out Guid guid))
        {
            return guid;
        }
        if (long.TryParse(alarmId, NumberStyles.Integer, CultureInfo.InvariantCulture, out long eventId))
        {
            return ToAlarmId(eventId);
        }
        throw new FormatException($"Alarm ID '{alarmId}' is not a RapidSCADA event ID.");
    }

    private static long FromAlarmId(Guid alarmId)
    {
        return BitConverter.ToInt64(alarmId.ToByteArray(), 0);
    }
}
