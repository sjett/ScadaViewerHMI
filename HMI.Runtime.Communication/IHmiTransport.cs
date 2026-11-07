// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Runtime.Communication;

public interface IHmiTransport : IAsyncDisposable
{
    string? AccessToken { get; set; }
    string? RefreshToken { get; set; }
    DateTime AccessTokenExpireTime { get; set; }

    event Func<Task>? OnReconnected;

    event Action<string, Exception, int>? OnCommunicationError;

    event Action<string, Exception, int, int[], string[]>? OnRegisterError;

    event Action<string, object>? OnStreamItemReceived;

    Task ConnectAsync(CancellationToken ct = default);

    Task<object?> InvokeAsync(
        string operation,
        object? payload,
        CancellationToken ct = default);

    Task StreamAsync(
        string operation,
        object? payload,
        CancellationToken ct = default);
}
