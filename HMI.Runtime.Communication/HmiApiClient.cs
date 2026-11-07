// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMI.Runtime.Communication;
/*
 static async Task Main()
{
    // 1. 创建 WebSocket 传输层（自动重连 + 自动认证 + 自动 Token 刷新）
    var transport = new WebSocketHmiTransport("wss://your-server:5443/ws/");

    // 2. 创建 API 客户端（封装 Login / RefreshToken / Authenticate / Subscribe）
    var client = new HmiApiClient(transport);

    // 3. 登录（自动保存 accessToken + refreshToken）
    Console.WriteLine("Logging in...");
    await client.LoginAsync("xl", "123456");
    Console.WriteLine("Login OK.");

    // 4. 订阅信号（自动记录订阅 → 自动恢复）
    Console.WriteLine("Subscribing...");
    CancellationTokenSource _cts = new();
    var updates = client.SubscribeSignalsAsync(
        "opc://opc.tcp://192.168.1.10:4840",
        new[] { "ns=2;s=Machine/Speed", "ns=2;s=Machine/State" }, _cts.Token
    );

    // 5. 启动后台任务处理订阅更新
    _ = Task.Run(async () =>
    {
        await foreach (var item in updates.WithCancellation(_cts.Token)) // WithCancellation(ct) 使得当 ct 取消时，订阅流会正确终止
        {
            if (item is SignalValue sv)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {sv.Name} = {sv.Value}");
            }
        }
    });

    // 6. 读写示例
    Console.WriteLine("Reading signal...");
    var speed = await client.ReadSignalAsync(
        "opc://opc.tcp://192.168.1.10:4840",
        "ns=2;s=Machine/Speed"
    );
    Console.WriteLine($"Speed = {speed}");

    Console.WriteLine("Writing signal...");
    await client.WriteSignalAsync(
        "opc://opc.tcp://192.168.1.10:4840",
        "ns=2;s=Machine/Speed",
        1200
    );
    Console.WriteLine("Write OK.");

    // 7. 程序保持运行
    Console.WriteLine("Running... Press ENTER to exit.");
    Console.ReadLine();

    _cts.Cancel();
    _cts.Dispose();
    await transport.DisposeAsync();
}

 */
public class HmiApiClient : IHmiApiClient
{
    private readonly IHmiTransport _transport;

    private readonly HashSet<string> _subscriptions = new(StringComparer.Ordinal);
    private readonly SemaphoreSlim _subscriptionLock = new(1, 1);

    public event Action<UserInfo>? OnUserContextChanged;
    public event Action<SignalValue>? SignalReceived;
    public event Action<string, Exception, int>? CommunicationError;
    public event Action<string, Exception, int, int[], string[]>? RegisterError;

    public HmiApiClient(IHmiTransport transport)
    {
        _transport = transport;

        _transport.OnReconnected += RestoreSubscriptionsAsync;
        _transport.OnStreamItemReceived += HandleStreamItem;
        _transport.OnCommunicationError += ForwardCommunicationError;
        _transport.OnRegisterError += ForwardRegisterError;
    }

    private void ForwardCommunicationError(string source, Exception error, int result)
    {
        CommunicationError?.Invoke(source, error, result);
    }

    private void ForwardRegisterError(string source, Exception error, int result, int[] signalResults, string[] signalNames)
    {
        RegisterError?.Invoke(source, error, result, signalResults, signalNames);
    }

    private void HandleStreamItem(string source, object item)
    {
        try
        {
            if (item is SignalValue signalValue)
            {
                SignalReceived?.Invoke(signalValue);
                return;
            }

            JsonElement payload = item switch
            {
                JsonElement element => element,
                string json => JsonDocument.Parse(json).RootElement.Clone(),
                _ => JsonSerializer.SerializeToElement(item)
            };

            if (payload.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement element in payload.EnumerateArray())
                {
                    PublishSignalValue(element);
                }
                return;
            }

            if (payload.ValueKind == JsonValueKind.Object &&
                payload.TryGetProperty("signals", out JsonElement signals) &&
                signals.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement element in signals.EnumerateArray())
                {
                    PublishSignalValue(element);
                }
                return;
            }

            PublishSignalValue(payload);
        }
        catch (JsonException exception)
        {
            CommunicationError?.Invoke(source, exception, -2);
        }
        catch (Exception exception)
        {
            CommunicationError?.Invoke(source, exception, -2);
        }
    }

    private void PublishSignalValue(JsonElement payload)
    {
        SignalValue? signalValue = payload.Deserialize<SignalValue>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (signalValue is not null && !string.IsNullOrWhiteSpace(signalValue.Name))
        {
            SignalReceived?.Invoke(signalValue);
        }
    }
    private async Task RestoreSubscriptionsAsync()
    {
        await _subscriptionLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_subscriptions.Count > 0)
            {
                await _transport.StreamAsync(
                    "SubscribeSignals",
                    new { signals = _subscriptions.ToArray() }).ConfigureAwait(false);
            }
        }
        finally
        {
            _subscriptionLock.Release();
        }
    }
    private async Task EnsureAccessTokenValidAsync(CancellationToken ct)
    {
        if (string.IsNullOrEmpty(_transport.AccessToken))
            return;

        // 未过期
        if (_transport.AccessTokenExpireTime > DateTime.UtcNow.AddSeconds(10))
            return;

        // 已过期 → 刷新
        var result = await InvokeTypedAsync<RefreshTokenResult>("RefreshToken", new
        {
            refreshToken = _transport.RefreshToken
        }, ct);

        _transport.AccessToken = result.AccessToken;
        _transport.RefreshToken = result.RefreshToken;
        _transport.AccessTokenExpireTime = DateTime.UtcNow.AddMinutes(result.ExpiresInMinutes);

        // 刷新后立即 Authenticate
        await AuthenticateAsync(result.AccessToken, ct);
    }


    // -------------------------
    // 登录 / 认证
    // -------------------------
    public async Task<UserInfo> LoginAsync(string user, string password, CancellationToken ct = default)
    {
        var result = await InvokeTypedAsync<LoginResult>("Login", new { user, password }, ct);

        _transport.AccessToken = result.AccessToken;
        _transport.RefreshToken = result.RefreshToken;
        _transport.AccessTokenExpireTime = DateTime.UtcNow.AddMinutes(result.ExpiresInMinutes);

        await AuthenticateAsync(result.AccessToken, ct);

        OnUserContextChanged?.Invoke(result.UserInfo);

        return result.UserInfo;
    }
    public Task AuthenticateAsync(string token, CancellationToken ct = default)
        => _transport.InvokeAsync("Authenticate", new { token }, ct);

    public Task LogoutAsync(CancellationToken ct = default)
        => _transport.InvokeAsync("Logout", new { }, ct);


    // -------------------------
    // 信号读写
    // -------------------------
    public async Task<object?> ReadSignalAsync(string signal, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<object?>("ReadSignal", new { signal }, ct);
    }

    public async Task<object?> WriteSignalAsync(string signal, object value, WriteOptions? options = null, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        var payload = new
        {
            signal,
            value,
            user = options?.User,
            password = options?.Password,
        };

        return await _transport.InvokeAsync("WriteSignal", payload, ct);
    }

    public async Task SubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);
        await _transport.ConnectAsync(ct).ConfigureAwait(false);

        ArgumentNullException.ThrowIfNull(signals);
        string[] requestedSignals = signals
            .Where(signal => !string.IsNullOrWhiteSpace(signal))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (requestedSignals.Length == 0)
        {
            return;
        }

        await _subscriptionLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            string[] newSignals = requestedSignals
                .Where(signal => _subscriptions.Add(signal))
                .ToArray();
            if (newSignals.Length == 0)
            {
                return;
            }

            try
            {
                await _transport.StreamAsync(
                    "SubscribeSignals",
                    new { signals = newSignals },
                    ct).ConfigureAwait(false);
            }
            catch
            {
                _subscriptions.ExceptWith(newSignals);
                throw;
            }
        }
        finally
        {
            _subscriptionLock.Release();
        }
    }

    public async Task UnsubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(signals);
        string[] requestedSignals = signals
            .Where(signal => !string.IsNullOrWhiteSpace(signal))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (requestedSignals.Length == 0)
        {
            return;
        }

        await _subscriptionLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            string[] activeSignals = requestedSignals
                .Where(_subscriptions.Contains)
                .ToArray();
            if (activeSignals.Length == 0)
            {
                return;
            }

            await _transport.InvokeAsync(
                "UnsubscribeSignals",
                new { signals = activeSignals },
                ct).ConfigureAwait(false);
            _subscriptions.ExceptWith(activeSignals);
        }
        finally
        {
            _subscriptionLock.Release();
        }
    }

    // -------------------------
    // 报警
    // -------------------------

    public async Task<IEnumerable<Alarm>> GetActiveAlarmsAsync(CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<IEnumerable<Alarm>>("GetActiveAlarms", new { }, ct);
    }

    public async Task<object?> AcknowledgeAlarmsAsync(IEnumerable<Guid> alarmIds, string? comment = null, WriteOptions? options = null, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        var payload = new
        {
            alarmIds = alarmIds.ToArray(),
            comment,
            user = options?.User,
            password = options?.Password
        };

        return await _transport.InvokeAsync("AcknowledgeAlarms", payload, ct);
    }
    public async Task<object?> AcknowledgeAlarmsByGroupAsync(string groupName, string? comment = null, WriteOptions? options = null, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        var payload = new
        {
            groupName,
            comment,
            user = options?.User,
            password = options?.Password
        };

        return await _transport.InvokeAsync("AcknowledgeAlarmsByGroup", payload, ct);
    }
    public async Task<object?> AcknowledgeAllAlarmsAsync(WriteOptions? options = null, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        var payload = new
        {
            comment = "报警已确认",
            user = options?.User,
            password = options?.Password
        };

        return await _transport.InvokeAsync("AcknowledgeAllAlarmsWithComment", payload, ct);
    }
    public async Task<object?> AcknowledgeAllAlarmsWithCommentAsync(string? comment = null, WriteOptions? options = null, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        var payload = new
        {
            comment,
            user = options?.User,
            password = options?.Password
        };

        return await _transport.InvokeAsync("AcknowledgeAllAlarmsWithComment", payload, ct);
    }
    public async Task<object?> ChangePasswordAsUserAsync(string username, string oldPassword, string newPassword, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await _transport.InvokeAsync("ChangePasswordAsUser", new { username, oldPassword, newPassword }, ct);
    }

    public async Task<object?> ChangePasswordAsAdminAsync(string username, string newPassword, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await _transport.InvokeAsync("ChangePasswordAsAdmin", new { username, newPassword }, ct);
    }

    public async Task<UserInfo> GetUserInfoAsync(CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<UserInfo>("GetUserInfo", new { }, ct);
    }

    public async Task<SecurityStatus> GetSecurityStatusAsync(CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<SecurityStatus>("GetSecurityStatus", new { }, ct);
    }

    // -------------------------
    // 多语言
    // -------------------------

    public async Task<IEnumerable<Language>> GetAllLanguages(CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<IEnumerable<Language>>("GetAllLanguages", new { }, ct);
    }

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetSymbolicTextTranslations(int languageId, CancellationToken ct = default)
    {
        await EnsureAccessTokenValidAsync(ct);

        return await InvokeTypedAsync<IEnumerable<KeyValuePair<string, string>>>("GetSymbolicTextTranslations", new { languageId }, ct);
    }

    // -------------------------
    // 通用强类型调用
    // -------------------------
    private async Task<T> InvokeTypedAsync<T>(string op, object payload, CancellationToken ct)
    {
        var result = await _transport.InvokeAsync(op, payload, ct);

        if (result is null)
            return default!;
        // 如果 transport 返回的是 JSON 字符串
        if (result is string json)
            return JsonSerializer.Deserialize<T>(json)!;

        // 如果 transport 返回的是 JsonElement
        if (result is JsonElement elem)
            return elem.Deserialize<T>()!;

        // 如果 transport 返回的是强类型对象（极少数情况）
        return (T)result;
    }

    public async ValueTask DisposeAsync()
    {
        _transport.OnReconnected -= RestoreSubscriptionsAsync;
        _transport.OnStreamItemReceived -= HandleStreamItem;
        _transport.OnCommunicationError -= ForwardCommunicationError;
        _transport.OnRegisterError -= ForwardRegisterError;
        await _transport.DisposeAsync().ConfigureAwait(false);
        _subscriptionLock.Dispose();
    }

    public Task<int> GetSecurtityStatusAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<object?> AcknowledgeAllGoneAlarmsAsync(string? comment = null, WriteOptions? options = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<object?> SetAlarmStateAsync(IEnumerable<string> alarmIds, int[] states, DateTime[] reactivations, WriteOptions? options = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }



    // -------------------------
    // 内部 DTO
    // -------------------------

    private sealed class LoginResult
    {
        public string AccessToken { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
        public int ExpiresInMinutes { get; init; }
        public UserInfo UserInfo { get; init; } = default!;
    }

    private sealed class RefreshTokenResult
    {
        public string AccessToken { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
        public int ExpiresInMinutes { get; init; }
    }
}
