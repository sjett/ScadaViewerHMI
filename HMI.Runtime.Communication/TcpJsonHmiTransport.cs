// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Runtime.Communication;

public class TcpJsonHmiTransport : IHmiTransport
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;

    public TcpJsonHmiTransport(string host, int port)
    {
        _client = new TcpClient();
        _client.Connect(host, port);
        _stream = _client.GetStream();
    }

    public async Task<object?> InvokeAsync(string operation, object? payload, CancellationToken ct = default)
    {
        var request = new
        {
            op = operation,
            data = payload
        };

        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json + "\n");
        await _stream.WriteAsync(bytes, 0, bytes.Length, ct);

        // 简单起见：一行一条响应
        using var reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, leaveOpen: true);
        var line = await reader.ReadLineAsync();
        if (line == null) return null;

        // 这里你可以定义统一响应格式，比如 { success, data, error }
        var doc = System.Text.Json.JsonDocument.Parse(line);
        var root = doc.RootElement;
        if (root.TryGetProperty("data", out var dataElement))
        {
            // 这里先返回 JsonElement，上层再做强类型转换
            return dataElement;
        }

        return null;
    }

    public async IAsyncEnumerable<object> StreamAsync(string operation, object? payload, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        // 如果你有长连接推送协议，可以在这里实现订阅逻辑
        throw new NotImplementedException();
    }

    public async ValueTask DisposeAsync()
    {
        _stream.Dispose();
        _client.Close();
        await Task.CompletedTask;
    }
}

