// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Communication;
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Core.Helper.Models;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Windows;
using System.Xml.Linq;

namespace HMI.Runtime.Core;

internal readonly record struct SignalSubscriptionKey(string ServerName, string SignalName);

internal sealed class SignalSubscription
{
    public object SyncRoot { get; } = new();
    public SemaphoreSlim OperationLock { get; } = new(1, 1);
    public HashSet<OnSignalChangedDelegate> Handlers { get; } = new();
    public int ExplicitReferenceCount { get; set; }
    public bool IsRemotelySubscribed { get; set; }

    public bool IsRequested
    {
        get
        {
            lock (SyncRoot)
            {
                return ExplicitReferenceCount > 0 || Handlers.Count > 0;
            }
        }
    }
}

public class HMIConnector : IHMIConnector
{
    private readonly ConcurrentDictionary<string, Lazy<IHmiApiClient>> _clients = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, ServerConfig> _configs;
    private readonly ConcurrentDictionary<SignalSubscriptionKey, object?> _signalValueCache = new();
    private readonly ConcurrentDictionary<SignalSubscriptionKey, SignalSubscription> _signalSubscriptions = new();
    private readonly IHmiApiClientFactory _apiClientFactory;
    private readonly int _subscriptionPollingIntervalMilliseconds;
    private UserInfo? _currentUserInfo;
    private string _password = string.Empty; // Retained for the convenience write overloads after login.

    private readonly ConcurrentDictionary<string, IHMITranslationProvider> _translations = new();

    private readonly IHMITrace trace;

    private string secrityServerName = string.Empty;
    private string translationServerName = string.Empty;

    public event OnUserContextChangedEventHandler? UserContextChanged;
    public event OnCommunicationErrorEventHandler? CommunicationError;
    public event OnRegisterErrorEventHandler? RegisterError;

    public Guid ClientId { get; set; }// 客户端标识，可以用于区分不同的客户端实例，特别是在多客户端环境中进行调试和日志记录时非常有用。
    public string SECURITY_BLOB { get; set; } = string.Empty; // 安全令牌，可以在需要进行安全认证的操作中使用，例如写信号时提供给后端进行验证，确保只有授权的客户端才能执行敏感操作。

    public HMIConnector(IHMITrace trace, ServerSettings settings)
        : this(trace, settings, new RapidScadaHmiApiClientFactory())
    {
    }

    public HMIConnector(IHMITrace trace, ServerSettings settings, IHmiApiClientFactory apiClientFactory)
    {
        this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
        ArgumentNullException.ThrowIfNull(settings);
        _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
        if (settings.SubscriptionPollingIntervalMilliseconds is < 100 or > 60000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings),
                "The subscription polling interval must be between 100 and 60000 milliseconds.");
        }
        _subscriptionPollingIntervalMilliseconds = settings.SubscriptionPollingIntervalMilliseconds;

        _configs = settings.Servers.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
        if (_configs.Count == 0)
        {
            throw new ArgumentException("At least one runtime server must be configured.", nameof(settings));
        }
        if (settings.DefaultServerName is not null && !_configs.ContainsKey(settings.DefaultServerName))
        {
            throw new ArgumentException(
                $"Default server '{settings.DefaultServerName}' is not configured.",
                nameof(settings));
        }
        DefaultServerName = settings.DefaultServerName;

        Statistics = new ConnectorStatistics(new CoreTimer());

    }
    private string? DefaultServerName { get; }

    private string ResolveServerName(string? serverName)
    {
        if (!string.IsNullOrWhiteSpace(serverName))
        {
            if (!_configs.ContainsKey(serverName))
            {
                throw new InvalidOperationException($"Server '{serverName}' is not configured.");
            }
            return _configs[serverName].Name;
        }

        if (!string.IsNullOrWhiteSpace(DefaultServerName))
        {
            return _configs[DefaultServerName].Name;
        }

        if (_configs.Count == 1)
        {
            return _configs.Values.Single().Name;
        }

        throw new InvalidOperationException(
            "A server name is required when multiple runtime servers are configured.");
    }

    private IHmiApiClient GetOrCreate(string serverName)
    {
        string resolvedServerName = ResolveServerName(serverName);
        Lazy<IHmiApiClient> client = _clients.GetOrAdd(
            resolvedServerName,
            name => new Lazy<IHmiApiClient>(
                () => CreateClient(name),
                LazyThreadSafetyMode.ExecutionAndPublication));
        return client.Value;
    }

    private IHmiApiClient CreateClient(string serverName)
    {
        ServerConfig config = _configs[serverName];
        if (string.IsNullOrWhiteSpace(config.Host) || config.Port is <= 0 or > 65535)
        {
            throw new InvalidOperationException(
                $"Server '{serverName}' must have a valid RapidSCADA host and port.");
        }

        IHmiApiClient client = _apiClientFactory.Create(new HmiApiConnectionOptions
        {
            Name = config.Name,
            Host = config.Host,
            Port = config.Port,
            Username = config.Username,
            Password = config.Password,
            Instance = config.Instance,
            SecretKey = config.SecretKey,
            Timeout = config.Timeout,
            SubscriptionPollingIntervalMilliseconds = _subscriptionPollingIntervalMilliseconds
        });
        BindEvents(client, serverName);
        if (string.IsNullOrEmpty(secrityServerName))
        {
            secrityServerName = serverName;
        }
        if (string.IsNullOrEmpty(translationServerName))
        {
            translationServerName = serverName;
        }
        return client;
    }
    private IHMITranslationProvider GetOrCreateTranslationProvider(string serverName)
    {
        serverName = ResolveServerName(serverName);
        if (_translations.TryGetValue(serverName, out var provider))
            return provider;
        var api = GetOrCreate(serverName);
        provider = new HMITranslationProvider(api);
        _translations[serverName] = provider;
        return provider;
    }

    private void BindEvents(IHmiApiClient api, string serverName)
    {
        api.OnUserContextChanged += OnUserContextChanged;
        api.SignalReceived += signalValue => OnSignalReceived(serverName, signalValue);
        api.CommunicationError += (_, error, result) =>
            CommunicationError?.Invoke(serverName, error, result);
        api.RegisterError += (_, error, result, signalResults, signalNames) =>
            RegisterError?.Invoke(serverName, error, result, signalResults, signalNames);
    }

    private void OnSignalReceived(string serverName, SignalValue signalValue)
    {
        SignalSubscriptionKey key = new(serverName, signalValue.Name);
        _signalValueCache[key] = signalValue.Value;
        if (!_signalSubscriptions.TryGetValue(key, out SignalSubscription? subscription))
        {
            return;
        }

        OnSignalChangedDelegate[] handlers;
        lock (subscription.SyncRoot)
        {
            handlers = subscription.Handlers.ToArray();
        }

        foreach (OnSignalChangedDelegate handler in handlers)
        {
            DispatchSignalChange(handler, signalValue.Name, signalValue.Value!);
        }
    }

    private void DispatchSignalChange(OnSignalChangedDelegate handler, string signalName, object? value)
    {
        void InvokeHandler()
        {
            try
            {
                handler(signalName, value!);
            }
            catch (Exception exception)
            {
                OutputDebugText(
                    HMIDebugLevel.Error,
                    "Signal change handler failed for '{0}': {1}",
                    signalName,
                    exception.Message);
            }
        }

        Application.Current?.Dispatcher.BeginInvoke((Action)InvokeHandler);
        if (Application.Current is null)
        {
            InvokeHandler();
        }
    }

    private void OnUserContextChanged(UserInfo info)
    {
        if (_currentUserInfo != info)
            _currentUserInfo = info;

        UserContextChanged?.Invoke(info.UserName);
    }

    // -------------------------
    // 报警
    // -------------------------

    public Task<IReadOnlyList<UnacknowledgedEvent>> GetUnacknowledgedEventsAsync(
        string serverName,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("The event query start time must precede its end time.", nameof(startTime));
        }

        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not IUnacknowledgedEventSource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not support querying unacknowledged events.");
        }

        return source.GetUnacknowledgedEventsAsync(startTime, endTime, cancellationToken);
    }

    public Task<IReadOnlyList<AlarmHistoryEvent>> GetAlarmHistoryAsync(
        string serverName,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("The event query start time must precede its end time.", nameof(startTime));
        }

        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not IAlarmHistorySource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not support querying alarm history.");
        }

        return source.GetAlarmHistoryAsync(startTime, endTime, cancellationToken);
    }

    public Task<IReadOnlyList<OperationEvent>> GetOperationEventsAsync(
        string serverName,
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

        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not IOperationEventSource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not support querying its event stream.");
        }

        return source.GetOperationEventsAsync(startTime, endTime, maxEvents, cancellationToken);
    }

    public Task<AlarmFilterMetadata> GetAlarmFilterMetadataAsync(
        string serverName,
        CancellationToken cancellationToken = default)
    {
        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not IAlarmMetadataSource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not support alarm group and type metadata.");
        }

        return source.GetAlarmFilterMetadataAsync(cancellationToken);
    }

    public Task<IReadOnlyList<HistoricalTrendSeries>> GetHistoricalTrendsAsync(
        string serverName,
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

        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not IHistoricalTrendSource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not support historical trend queries.");
        }

        return source.GetHistoricalTrendsAsync(signalNames, startTime, endTime, archiveBit, cancellationToken);
    }

    public Task<IReadOnlyList<string>> GetSignalNamesAsync(
        string serverName,
        CancellationToken cancellationToken = default)
    {
        IHmiApiClient client = GetOrCreate(serverName);
        if (client is not ISignalCatalogSource source)
        {
            throw new NotSupportedException($"Server '{ResolveServerName(serverName)}' does not provide a signal catalog.");
        }

        return source.GetSignalNamesAsync(cancellationToken);
    }

    // -------------------------
    // 生命周期
    // -------------------------

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (Lazy<IHmiApiClient> client in _clients.Values)
        {
            if (client.IsValueCreated)
            {
                await client.Value.DisposeAsync().ConfigureAwait(false);
            }
        }
        _clients.Clear();

        foreach (var t in _translations.Values)
            t.Dispose();
        _translations.Clear();

        foreach (SignalSubscription subscription in _signalSubscriptions.Values)
        {
            subscription.OperationLock.Dispose();
        }
        _signalSubscriptions.Clear();
        _signalValueCache.Clear();
    }

    public IConnectorStatistics Statistics { get; private set; }

    #region 语言设置,提供了多种重载以适应不同的使用场景
    public int GetLanguageId()
    {
        return GetOrCreateTranslationProvider(translationServerName).GetLanguageId();
    }

    public HMILanguage[] ReadLanguages()
    {
        return GetOrCreateTranslationProvider(translationServerName).ReadLanguages();
    }
    public void SetLanguageId(int languageId)
    {
        GetOrCreateTranslationProvider(translationServerName).SetLanguageId(languageId);
    }
    public void RegisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        GetOrCreateTranslationProvider(translationServerName).RegisterLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate);
    }
    public void UnregisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        GetOrCreateTranslationProvider(translationServerName).UnregisterLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate);
    }
    #endregion

    #region 信号读取,提供了多种重载以适应不同的使用场景
    public object? GetSignalValue(string serverName, string signalName)
    {
        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        if (_signalValueCache.TryGetValue(key, out var value))
            return value;

        return null;
    }

    public void RegisterSignal(string serverName, string signalName)
    {
        if (string.IsNullOrWhiteSpace(signalName))
        {
            return;
        }

        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        SignalSubscription subscription = _signalSubscriptions.GetOrAdd(key, _ => new SignalSubscription());
        lock (subscription.SyncRoot)
        {
            subscription.ExplicitReferenceCount++;
        }
        _ = SynchronizeSignalSubscriptionAsync(key, subscription);
    }
    /// <summary>
    /// 取消注册信号，当某个信号的引用计数为0时，会从本地缓存中移除该信号的值。
    /// </summary>
    /// <param name="serverName"></param>
    /// <param name="signalName"></param>
    public void UnregisterSignal(string serverName, string signalName)
    {
        if (string.IsNullOrWhiteSpace(signalName))
        {
            return;
        }

        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        if (!_signalSubscriptions.TryGetValue(key, out SignalSubscription? subscription))
        {
            return;
        }

        lock (subscription.SyncRoot)
        {
            if (subscription.ExplicitReferenceCount > 0)
            {
                subscription.ExplicitReferenceCount--;
            }
        }
        _ = SynchronizeSignalSubscriptionAsync(key, subscription);
    }

    public void RegisterSignalChangedHandler(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate)
    {
        if (string.IsNullOrWhiteSpace(signalName) || onSignalChangedDelegate is null)
        {
            return;
        }

        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        SignalSubscription subscription = _signalSubscriptions.GetOrAdd(key, _ => new SignalSubscription());
        bool added;
        lock (subscription.SyncRoot)
        {
            added = subscription.Handlers.Add(onSignalChangedDelegate);
        }
        if (!added)
        {
            return;
        }

        if (_signalValueCache.TryGetValue(key, out object? currentValue))
        {
            DispatchSignalChange(onSignalChangedDelegate, signalName, currentValue);
        }
        _ = SynchronizeSignalSubscriptionAsync(key, subscription);
    }

    public void UnregisterSignalChangedHandler(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate)
    {
        if (onSignalChangedDelegate is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(signalName))
        {
            string? resolvedServerName = string.IsNullOrWhiteSpace(serverName)
                ? null
                : ResolveServerName(serverName);
            foreach (KeyValuePair<SignalSubscriptionKey, SignalSubscription> entry in _signalSubscriptions)
            {
                if (resolvedServerName is not null &&
                    !string.Equals(entry.Key.ServerName, resolvedServerName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                RemoveSignalHandler(entry.Key, entry.Value, onSignalChangedDelegate);
            }
            return;
        }

        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        if (_signalSubscriptions.TryGetValue(key, out SignalSubscription? subscription))
        {
            RemoveSignalHandler(key, subscription, onSignalChangedDelegate);
        }
    }

    private void RemoveSignalHandler(
        SignalSubscriptionKey key,
        SignalSubscription subscription,
        OnSignalChangedDelegate handler)
    {
        lock (subscription.SyncRoot)
        {
            subscription.Handlers.Remove(handler);
        }
        _ = SynchronizeSignalSubscriptionAsync(key, subscription);
    }

    private async Task SynchronizeSignalSubscriptionAsync(
        SignalSubscriptionKey key,
        SignalSubscription subscription)
    {
        try
        {
            await subscription.OperationLock.WaitAsync().ConfigureAwait(false);
            try
            {
                while (true)
                {
                    bool isRequested = subscription.IsRequested;
                    if (isRequested == subscription.IsRemotelySubscribed)
                    {
                        if (!isRequested)
                        {
                            _signalValueCache.TryRemove(key, out _);
                        }
                        return;
                    }

                    IHmiApiClient client = GetOrCreate(key.ServerName);
                    if (isRequested)
                    {
                        await client.SubscribeSignalsAsync([key.SignalName]).ConfigureAwait(false);
                    }
                    else
                    {
                        await client.UnsubscribeSignalsAsync([key.SignalName]).ConfigureAwait(false);
                    }

                    subscription.IsRemotelySubscribed = isRequested;
                }
            }
            finally
            {
                subscription.OperationLock.Release();
            }
        }
        catch (Exception exception)
        {
            CommunicationError?.Invoke(key.ServerName, exception, -1);
        }
    }

    public void RegisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate)
    {
        RegisterSignalInformationChangedHandler(serverName, new string[]
            {
                signalName
            }, onSignalInformationChangedDelegate);
    }

    public void RegisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate)
    {
        //todo..
        onSignalInformationChangedDelegate(serverName, signalNames, signalNames.Select(n => new HMISignalDefinition { Name = n }).ToArray());
    }
    public void UnregisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate)
    {
        UnregisterSignalInformationChangedHandler(serverName, new string[]
            {
                signalName
            }, onSignalInformationChangedDelegate);
    }

    public void UnregisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate)
    {
        //todo..
        onSignalInformationChangedDelegate(serverName, signalNames, signalNames.Select(n => new HMISignalDefinition { Name = n }).ToArray());
    }
    #endregion

    #region 信号写入,提供了多种重载以适应不同的使用场景
    public void ToggleSignal(string serverName, string signalName, object onValue, object offValue)
    {
        ToggleSignal(serverName, signalName, onValue, offValue, null);
    }

    public void ToggleSignal(string serverName, string signalName, object onValue, object offValue, OnResultDelegate? resultDelegate)
    {
        SignalSubscriptionKey key = new(ResolveServerName(serverName), signalName);
        if (!_signalValueCache.TryGetValue(key, out var current))
            return;

        double cur = 0.0;
        double on = 0.0;
        double off = 0.0;
        try
        {
            cur = Convert.ToDouble(current);
            on = Convert.ToDouble(onValue);
            off = Convert.ToDouble(offValue);
        }
        catch
        {
            cur = 0.0;
        }

        if (cur == off)
        {
            WriteSignal(serverName, signalName, on, resultDelegate);
        }
        else
        {
            WriteSignal(serverName, signalName, off, resultDelegate);
        }
    }

 
    public void WriteSignal(string serverName, string signalName, object value)
    {
        WriteSignal(serverName, signalName, value, (OnResultDelegate?)null);
    }

    /// <summary>
    /// 写信号的基础方法，其他重载最终都会调用这个方法。
    /// 它会将写信号的结果通过 resultDelegate 回调返回，
    /// resultDelegate 的参数是一个包含错误信息（如果有的话）和一个整数数组（可以包含状态码等信息）的元组。
    /// </summary>
    /// <param name="serverName"></param>
    /// <param name="signalName"></param>
    /// <param name="value"></param>
    /// <param name="resultDelegate"></param>
    public void WriteSignal(string serverName, string signalName, object value, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(serverName).WriteSignalAsync(signalName, value, null).ContinueWith(t =>
        {
            if (t.IsCompletedSuccessfully)
            {
                resultDelegate?.Invoke(null, [0]);
            }
            else
            {
                resultDelegate?.Invoke(t.Exception, [1]);
            }
        });
    }

    public void WriteSignal(string serverName, string signalName, object value, string user, string password)
    {
        WriteSignal(serverName, signalName, value, user, password, false);
    }

    public void WriteSignal(string serverName, string signalName, object value, string user, string password, OnResultDelegate? resultDelegate)
    {
        WriteSignal(serverName, signalName, value, user, password, false, resultDelegate);
    }

    public void WriteSignal(string serverName, string signalName, object value, string user, string password, bool isDomainUser)
    {
        WriteSignal(serverName, signalName, value, user, password, isDomainUser, null);
    }
    /// <summary>
    /// 写信号的完整方法，包含了用户认证信息和结果回调。
    /// </summary>
    /// <param name="serverName"></param>
    /// <param name="signalName"></param>
    /// <param name="value"></param>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="isDomainUser"></param>
    /// <param name="resultDelegate"></param>
    public void WriteSignal(string serverName, string signalName, object value, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {

        if (signalName == null)
        {
            signalName = "";
        }

        //object obj = this.convertValue(value); // 目前先不做转换，直接传原值，后端根据实际类型进行处理

        var opt = new WriteOptions 
        { 
            User = user, 
            Password = password,
            //IsDomainUser = isDomainUser,
        };

        GetOrCreate(serverName).WriteSignalAsync(signalName, value, opt).ContinueWith(t =>
        {
            if (t.IsCompletedSuccessfully)
            {
                resultDelegate?.Invoke(null, [0]);
            }
            else
            {
                resultDelegate?.Invoke(t.Exception, [1]);// 错误代码表(简化版)： 0 - 成功，1 - 失败（其他错误）
            }
        });

        OutputDebugText(HMIDebugLevel.Warning, "Wrote signal : {0} with values : {1}", new object[]
        {
                signalName,
                value
        });
    }

    public void WriteSignal(string serverName, string signalName, object value, string password)
    {
        WriteSignal(serverName, signalName, value, getUserName(), password, false);
    }

    public void WriteSignal(string serverName, string signalName, object value, string password, OnResultDelegate resultDelegate)
    {
        WriteSignal(serverName, signalName, value, getUserName(), password, false, resultDelegate);
    }
    #endregion

    #region 信号状态设置,提供了多种重载以适应不同的使用场景
    public void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, [ signalName ], [ state ], null, user, password, isDomainUser, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, OnResultDelegate resultDelegate)
    {
        SetSignalState(serverName, signalName, state, user, password, false, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, [signalName],  [ state ], null, null, null, false, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, bool isDomainUser)
    {
        SetSignalStates(serverName, [signalName], [state], null, user, password, isDomainUser, null);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password)
    {
        SetSignalState(serverName, signalName, state, user, password, false);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state)
    {
        SetSignalStates(serverName,  [signalName],  [state], null, null, null, false, null);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, OnResultDelegate resultDelegate)
    {
        SetSignalState(serverName, signalName, state, inactiveValue, user, password, false, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, [signalName], [state], [inactiveValue], user, password, isDomainUser, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, [signalName], [state], [inactiveValue], null, null, false, resultDelegate);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, bool isDomainUser)
    {
        SetSignalStates(serverName, [signalName], [state], [inactiveValue], user, password, isDomainUser, null);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password)
    {
        SetSignalState(serverName, signalName, state, inactiveValue, user, password, false);
    }

    public void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue)
    {
        SetSignalStates(serverName, [signalName], [state], [inactiveValue], null, null, false, null);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, OnResultDelegate? resultDelegate)
    {
        SetSignalStates(serverName, signalNames, states, user, password, false, resultDelegate);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        SetSignalStates(serverName, signalNames, states, null, user, password, isDomainUser, resultDelegate);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, signalNames, states, null, null, null, false, resultDelegate);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password)
    {
        SetSignalStates(serverName, signalNames, states, user, password, false);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, bool isDomainUser)
    {
        SetSignalStates(serverName, signalNames, states, null, user, password, isDomainUser, null);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states)
    {
        SetSignalStates(serverName, signalNames, states, null, null, null, false, null);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, signalNames, states, inactiveValues, user, password, false, resultDelegate);
    }
    /// <summary>
    /// 批量设置信号状态的完整方法，包含了用户认证信息和结果回调。
    /// </summary>
    /// <param name="serverName"></param>
    /// <param name="signalNames"></param>
    /// <param name="states"></param>
    /// <param name="inactiveValues"></param>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="isDomainUser"></param>
    /// <param name="resultDelegate"></param>
    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[]? inactiveValues, string? user, string? password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        //todo..
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, OnResultDelegate resultDelegate)
    {
        SetSignalStates(serverName, signalNames, states, inactiveValues, null, null, false, resultDelegate);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password)
    {
        SetSignalStates(serverName, signalNames, states, inactiveValues, user, password, false);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password, bool isDomainUser)
    {
        SetSignalStates(serverName, signalNames, states, inactiveValues, user, password, isDomainUser, null);
    }

    public void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues)
    {
        SetSignalStates(serverName, signalNames, states, inactiveValues, null, null, false, null);
    }
    #endregion

    #region 报警确认,提供了多种重载以适应不同的使用场景

    #region AcknowledgeAlarms
    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        var opts = new WriteOptions
        {
            User = user,
            Password = password,
            //IsDomainUser = isDomainUser
        };

        GetOrCreate(serverName)
            .AcknowledgeAlarmsAsync(alarmIds, comment, opts)
            .ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        resultDelegate?.Invoke(null, [0]);
                    }
                    else
                    {
                        resultDelegate?.Invoke(t.Exception, [1]);
                    }
                });
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, (OnResultDelegate?)null);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, _password, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string password)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, password, (OnResultDelegate?)null);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, getUserName(), password, false, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, user, password, false, null);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, user, password, false, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, bool isDomainUser)
    {
        AcknowledgeAlarms(serverName, alarmIds, comment, user, password, isDomainUser, null);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, user, password, isDomainUser, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string password)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, password);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string password, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, password, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, user, password);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, user, password, resultDelegate);
    }

    public void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, bool isDomainUser)
    {
        AcknowledgeAlarms(serverName, [alarmId], comment, user, password, isDomainUser);
    }
    #endregion

    #region AcknowledgeAlarmsByGroup

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        var opts = new WriteOptions
        {
            User = user,
            Password = password,
            //IsDomainUser = isDomainUser
        };

        GetOrCreate(serverName)
            .AcknowledgeAlarmsByGroupAsync(groupName, comment, opts)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, (OnResultDelegate?)null);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, _password, resultDelegate);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string password)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, password, (OnResultDelegate?)null);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, getUserName(), password, false, resultDelegate);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, user, password, false, null);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, OnResultDelegate resultDelegate)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, user, password, false, resultDelegate);
    }

    public void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, bool isDomainUser)
    {
        AcknowledgeAlarmsByGroup(serverName, groupName, comment, user, password, isDomainUser, null);
    }

    #endregion

    #region AcknowledgeAllAlarms
    public void AcknowledgeAllAlarms(string serverName, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        var opts = new WriteOptions
        {
            User = user,
            Password = password,
            //IsDomainUser = isDomainUser
        };

        GetOrCreate(serverName)
            .AcknowledgeAllAlarmsAsync(opts)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllAlarms(string serverName)
    {
        AcknowledgeAllAlarms(serverName, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllAlarms(string serverName, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(serverName)
            .AcknowledgeAllAlarmsAsync(null)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllAlarms(string serverName, string password)
    {
        AcknowledgeAllAlarms(serverName, password, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllAlarms(string serverName, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllAlarms(serverName, getUserName(), password, resultDelegate);
    }

    public void AcknowledgeAllAlarms(string serverName, string user, string password)
    {
        AcknowledgeAllAlarms(serverName, user, password, null);
    }

    public void AcknowledgeAllAlarms(string serverName, string user, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllAlarms(serverName, user, password, false, resultDelegate);
    }

    public void AcknowledgeAllAlarms(string serverName, string user, string password, bool isDomainUser)
    {
        AcknowledgeAllAlarms(serverName, user, password, isDomainUser, null);
    }

    #endregion

    #region AcknowledgeAllAlarmsWithComment
    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        var opts = new WriteOptions
        {
            User = user,
            Password = _password,
            //IsDomainUser = false
        };
        GetOrCreate(serverName)
            .AcknowledgeAllAlarmsWithCommentAsync(comment, null)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(serverName)
            .AcknowledgeAllAlarmsWithCommentAsync(comment, null)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string password)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, password, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, getUserName(), password, resultDelegate);
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, user, password, false, null);
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, user, password, false, resultDelegate);
    }

    public void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, bool isDomainUser)
    {
        AcknowledgeAllAlarmsWithComment(serverName, comment, user, password, isDomainUser, null);
    }

    #endregion

    #region AcknowledgeAllGoneAlarms
    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        var opts = new WriteOptions
        {
            User = user,
            Password = password,
            //IsDomainUser = isDomainUser
        };
        GetOrCreate(serverName)
            .AcknowledgeAllGoneAlarmsAsync(comment, opts)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(serverName)
            .AcknowledgeAllGoneAlarmsAsync(comment, null)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string password)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, password, (OnResultDelegate?)null);
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, getUserName(), password, resultDelegate);
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, user, password, null);
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, OnResultDelegate? resultDelegate)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, user, password, false, resultDelegate);
    }

    public void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, bool isDomainUser)
    {
        AcknowledgeAllGoneAlarms(serverName, comment, user, password, isDomainUser, null);
    }

    #endregion

    #endregion

    #region 报警状态设置,提供了多种重载以适应不同的使用场景
    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        SetAlarmStates(serverName, [AlarmId], [state], [reactivation], user, password, isDomainUser, resultDelegate);
    }

    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, OnResultDelegate? resultDelegate)
    {
        SetAlarmState(serverName, AlarmId, state, reactivation, user, password, false, resultDelegate);
    }

    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, OnResultDelegate? resultDelegate)
    {
        SetAlarmStates(serverName, [AlarmId], [state], [reactivation], null, null, false, resultDelegate);
    }

    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, bool isDomainUser)
    {
        SetAlarmStates(serverName, [AlarmId], [state], [reactivation], user, password, isDomainUser, null);
    }

    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password)
    {
        SetAlarmState(serverName, AlarmId, state, reactivation, user, password, false);
    }

    public void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation)
    {
        SetAlarmStates(serverName, [AlarmId], [state], [reactivation], null);
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string? user, string? password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        WriteOptions? opts = null;
        if (password != null)
        {
            opts = new WriteOptions
            {
                User = user ?? getUserName(),
                Password = password,
                //IsDomainUser = isDomainUser
            };
        }
        GetOrCreate(serverName)
            .SetAlarmStateAsync(AlarmIds, states.Select(t=>(int)t).ToArray(), reactivations, opts)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password, OnResultDelegate? resultDelegate)
    {
        SetAlarmStates(serverName, AlarmIds, states, reactivations, user, password, false, resultDelegate);
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations)
    {
        SetAlarmStates(serverName, AlarmIds, states, reactivations, null, null, false, null);
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, OnResultDelegate? resultDelegate)
    {
        SetAlarmStates(serverName, AlarmIds, states, reactivations, null, null, false, resultDelegate);
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password, bool isDomainUser)
    {
        SetAlarmStates(serverName, AlarmIds, states, reactivations, user, password, isDomainUser, null);
    }

    public void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password)
    {
        SetAlarmStates(serverName, AlarmIds, states, reactivations, user, password, false);
    }
    #endregion

    #region 用户与权限,提供了多种重载以适应不同的使用场景
    public void ChangePasswordAsAdmin(string user, string newPassword)
    {
        ChangePasswordAsAdmin(user, newPassword, false);
    }

    public void ChangePasswordAsAdmin(string user, string newPassword, bool isDomainUser)
    {
        ChangePasswordAsAdmin(user, newPassword, isDomainUser, null);
    }

    public void ChangePasswordAsAdmin(string user, string newPassword, OnResultDelegate? resultDelegate)
    {
        ChangePasswordAsAdmin(user, newPassword, false, null);
    }

    public void ChangePasswordAsAdmin(string user, string newPassword, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(secrityServerName)
            .ChangePasswordAsAdminAsync(user, Cryptography.Encrypt(newPassword, "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179"))
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void ChangePasswordAsUser(string oldPassword, string newPassword)
    {
        ChangePasswordAsUser(oldPassword, newPassword, false);
    }

    public void ChangePasswordAsUser(string oldPassword, string newPassword, bool isDomainUser)
    {
        ChangePasswordAsUser(oldPassword, newPassword, isDomainUser, null);
    }

    public void ChangePasswordAsUser(string oldPassword, string newPassword, OnResultDelegate? resultDelegate)
    {
        ChangePasswordAsUser(oldPassword, newPassword, false, resultDelegate);
    }

    public void ChangePasswordAsUser(string oldPassword, string newPassword, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        ChangePasswordAsUser(getUserName(), oldPassword, newPassword, false, resultDelegate);
    }

    public void ChangePasswordAsUser(string username, string oldPassword, string newPassword)
    {
        ChangePasswordAsUser(username, oldPassword, newPassword, false);
    }

    public void ChangePasswordAsUser(string username, string oldPassword, string newPassword, bool isDomainUser)
    {
        ChangePasswordAsUser(username, oldPassword, newPassword, isDomainUser, null);
    }

    public void ChangePasswordAsUser(string username, string oldPassword, string newPassword, OnResultDelegate? resultDelegate)
    {
        ChangePasswordAsUser(username, oldPassword, newPassword, false, resultDelegate);
    }

    public void ChangePasswordAsUser(string username, string oldPassword, string newPassword, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(secrityServerName)
            .ChangePasswordAsUserAsync(
                username, 
                Cryptography.Encrypt(oldPassword, "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179"), 
                Cryptography.Encrypt(newPassword, "a45e6a42-1bfa-425e-9d35-dc4d4a3d0179"))
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public bool CheckAuthorizationGroup(string AuthorizationGroupName)
    {
        var array = GetAuthorizationGroups();
        for (int i = 0; i < array.Count; i++)
        {
            string text = array[i];
            if (text == AuthorizationGroupName)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckProjectAuthorization(string AuthorizationName)
    {
        var array = GetProjectAuthorizations();
        for (int i = 0; i < array.Count; i++)
        {
            string text = array[i];
            if (text == AuthorizationName)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckSystemAuthorization(string AuthorizationName)
    {
        var array = GetSystemAuthorizations();
        for (int i = 0; i < array.Count; i++)
        {
            string text = array[i];
            if (text == AuthorizationName)
            {
                return true;
            }
        }
        return false;
    }
    public List<string> GetAuthorizationGroups()
    {
        return _currentUserInfo?.AuthorizationGroups.ToList() ?? new List<string>();
    }
    public bool GetIsDomainUser()
    {
        return false; //暂不区分域用户和本地用户，默认都返回false

        return _currentUserInfo?.IsDomainUser ?? false;
    }
    public List<string> GetProjectAuthorizations()
    {
        return _currentUserInfo?.ProjectAuthorizations.ToList() ?? new List<string>();

    }
    public List<string> GetSystemAuthorizations()
    {
        return _currentUserInfo?.SystemAuthorizations.ToList() ?? new List<string>();
    }

    public int GetUserLevel()
    {
        return _currentUserInfo?.UserLevel ?? -1;
    }

    public string GetUserName()
    {
        return _currentUserInfo?.UserName ?? string.Empty;
    }

    public void LoginCurrentWindowsUser(OnResultDelegate resultDelegate)
    {
        //todo: 跨平台这里暂不实现自动登录当前Windows用户的功能，直接调用登录方法让用户输入用户名和密码进行登录
    }

    public void LoginUser(string user, string password)
    {
        LoginUser(user, password, false);
    }

    public void LoginUser(string user, string password, bool isDomainUser)
    {
        LoginUser(user, password, isDomainUser, null);
    }

    public void LoginUser(string user, string password, OnResultDelegate? resultDelegate)
    {
        LoginUser(user, password, false, null);
    }

    public void LoginUser(string user, string password, bool isDomainUser, OnResultDelegate? resultDelegate)
    {
        GetOrCreate(secrityServerName)
            .LoginAsync(user, password)
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    _password = password;
                    _currentUserInfo = t.Result;// 登录成功后保存当前用户信息

                    resultDelegate?.Invoke(null, [0]);
                }
                else
                {
                    resultDelegate?.Invoke(t.Exception, [1]);
                }
            });
    }

    public void LogoutUser()
    {
        GetOrCreate(secrityServerName)
            .LogoutAsync()
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    _password = "";// 注销成功后清除密码
                    _currentUserInfo = null;// 注销成功后清除当前用户信息
                }
                else
                {
                }
            });
    }
    public HMISecurityStatus GetSecurtityStatus()
    {
        GetOrCreate(secrityServerName)
            .GetSecurtityStatusAsync()
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    return (HMISecurityStatus)t.Result;
                }
                else
                {
                    return HMISecurityStatus.SecurityUnknown;
                }
            });

        return HMISecurityStatus.SecurityUnknown;
    }
    public void SetSecurityServer(string serverName)
    {
        if (!string.IsNullOrEmpty(secrityServerName))
        {
            GetOrCreate(secrityServerName).OnUserContextChanged -= OnUserContextChanged;
            secrityServerName = string.Empty;
        }

        GetOrCreate(serverName);
        secrityServerName = serverName;
    }
    #endregion
    private string getUserName()
    {
        string? userName = _currentUserInfo?.UserName;
        if (userName == null)
        {
            return string.Empty;
        }
        return userName;
    }
    public void OutputDebugText(HMIDebugLevel level, string textFormat, params object[] textFormatParams)
    {
        if (trace != null)
        {
            trace.OutputDebugText(level, textFormat, textFormatParams);
        }
    }

}
