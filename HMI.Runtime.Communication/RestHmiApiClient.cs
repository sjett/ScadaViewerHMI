// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Runtime.Communication;

public class RestHmiApiClient : IHmiApiClient
{
    private readonly HttpClient _http;

    public RestHmiApiClient(HttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<object?> ReadSignalAsync(string server, string signal, CancellationToken ct = default)
    {
        var response = await _http.GetAsync($"/api/servers/{server}/signals/{signal}", ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct);
        // 这里可以根据你的约定反序列化
        return System.Text.Json.JsonSerializer.Deserialize<object?>(json);
    }

    public async Task WriteSignalAsync(string server, string signal, object value, WriteOptions? options = null, CancellationToken ct = default)
    {
        var payload = new
        {
            value,
            user = options?.User,
            password = options?.Password,
            isDomainUser = options?.IsDomainUser
        };

        var response = await _http.PostAsJsonAsync($"/api/servers/{server}/signals/{signal}", payload, ct);
        response.EnsureSuccessStatusCode();

        if (options?.Callback != null)
        {
            // 如果服务端返回结果数组，可在此解析
            options.Callback((null, null));
        }
    }

    // 其他方法同理：调用 /api/alarms/... /api/auth/... 等
    public IAsyncEnumerable<SignalValue> SubscribeSignalsAsync(string server, IEnumerable<string> signals, CancellationToken ct = default)
        => throw new NotImplementedException(); // 交给 SignalR 实现

    public Task<IEnumerable<Alarm>> GetActiveAlarmsAsync(string server, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task AcknowledgeAlarmsAsync(string server, IEnumerable<Guid> alarmIds, string? comment = null, WriteOptions? options = null, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<UserInfo> LoginAsync(string server, string user, string password, bool isDomainUser = false, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<UserInfo> LoginCurrentWindowsUserAsync(string server, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task LogoutAsync(string server, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task ChangePasswordAsUserAsync(string server, string username, string oldPassword, string newPassword, bool isDomainUser = false, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task ChangePasswordAsAdminAsync(string server, string username, string newPassword, bool isDomainUser = false, CancellationToken ct = default)
        => throw new NotImplementedException();
}

