// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace HMI.Runtime.Communication;

public sealed class WebSocketHmiTransport : IHmiTransport
{
    private readonly Uri _uri;
    private readonly CancellationTokenSource _disposeTokenSource = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<object?>> _pending = new();
    private readonly Channel<object?> _streamItems = Channel.CreateUnbounded<object?>();
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly TimeSpan _reconnectDelay = TimeSpan.FromSeconds(3);

    private ClientWebSocket _webSocket = new();
    private Task? _receiveLoopTask;
    private readonly Task _streamDispatchTask;
    private int _hasConnected;
    private int _disposed;

    public event Func<Task>? OnReconnected;
    public event Action<string, Exception, int>? OnCommunicationError;
    public event Action<string, Exception, int, int[], string[]>? OnRegisterError;
    public event Action<string, object>? OnStreamItemReceived;

    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime AccessTokenExpireTime { get; set; }

    public WebSocketHmiTransport(Uri uri)
    {
        _uri = uri ?? throw new ArgumentNullException(nameof(uri));
        if (!_uri.IsAbsoluteUri ||
            (_uri.Scheme != Uri.UriSchemeWs && _uri.Scheme != Uri.UriSchemeWss))
        {
            throw new ArgumentException("The endpoint must be an absolute ws:// or wss:// URI.", nameof(uri));
        }

        _streamDispatchTask = ProcessStreamItemsAsync(_disposeTokenSource.Token);
    }

    public async Task ConnectAsync(CancellationToken ct = default)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        await _connectionLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                return;
            }

            if (_webSocket.State != WebSocketState.None)
            {
                _webSocket.Dispose();
                _webSocket = new ClientWebSocket();
            }

            await _webSocket.ConnectAsync(_uri, ct).ConfigureAwait(false);
            ClientWebSocket connectedSocket = _webSocket;
            bool isReconnect = Interlocked.Exchange(ref _hasConnected, 1) != 0;
            _receiveLoopTask = Task.Run(() => ReceiveLoopAsync(connectedSocket, _disposeTokenSource.Token));

            if (isReconnect)
            {
                await NotifyReconnectedAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task<object?> InvokeAsync(string operation, object? payload, CancellationToken ct = default)
    {
        await EnsureConnectedAsync(ct).ConfigureAwait(false);

        string requestId = Guid.NewGuid().ToString("N");
        TaskCompletionSource<object?> completion =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!_pending.TryAdd(requestId, completion))
        {
            throw new InvalidOperationException("Failed to register a pending WebSocket request.");
        }

        try
        {
            await SendMessageAsync(new
            {
                type = "invoke",
                op = operation,
                requestId,
                payload,
                accessToken = AccessToken
            }, ct).ConfigureAwait(false);

            using CancellationTokenRegistration registration =
                ct.Register(() => completion.TrySetCanceled(ct));
            return await completion.Task.ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
            throw;
        }
        finally
        {
            _pending.TryRemove(requestId, out _);
        }
    }

    public async Task StreamAsync(string operation, object? payload, CancellationToken ct = default)
    {
        await EnsureConnectedAsync(ct).ConfigureAwait(false);
        string requestId = Guid.NewGuid().ToString("N");

        try
        {
            await SendMessageAsync(new
            {
                type = "stream",
                op = operation,
                requestId,
                payload,
                accessToken = AccessToken
            }, ct).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
            throw;
        }
    }

    private async Task SendMessageAsync(object message, CancellationToken ct)
    {
        byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(message);
        await _sendLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            ClientWebSocket socket = _webSocket;
            if (socket.State != WebSocketState.Open)
            {
                throw new WebSocketException("The WebSocket connection is not open.");
            }
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, ct).ConfigureAwait(false);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    private async Task EnsureConnectedAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && Volatile.Read(ref _disposed) == 0)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                return;
            }

            try
            {
                await ConnectAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (WebSocketException exception)
            {
                OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
                await Task.Delay(_reconnectDelay, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
                await Task.Delay(_reconnectDelay, ct).ConfigureAwait(false);
            }
        }

        ct.ThrowIfCancellationRequested();
        throw new ObjectDisposedException(nameof(WebSocketHmiTransport));
    }

    private async Task ReceiveLoopAsync(ClientWebSocket socket, CancellationToken ct)
    {
        byte[] buffer = new byte[16 * 1024];
        Exception? connectionError = null;

        try
        {
            while (!ct.IsCancellationRequested && socket.State == WebSocketState.Open)
            {
                using MemoryStream message = new();
                ValueWebSocketReceiveResult result;
                do
                {
                    result = await socket.ReceiveAsync(buffer.AsMemory(), ct).ConfigureAwait(false);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseOutputAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "connection closed",
                            CancellationToken.None).ConfigureAwait(false);
                        return;
                    }

                    message.Write(buffer, 0, result.Count);
                }
                while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    HandleMessage(Encoding.UTF8.GetString(message.ToArray()));
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return;
        }
        catch (WebSocketException exception)
        {
            connectionError = exception;
        }
        catch (JsonException exception)
        {
            connectionError = exception;
        }
        finally
        {
            if (!ct.IsCancellationRequested && Volatile.Read(ref _disposed) == 0)
            {
                await HandleConnectionClosedAsync(connectionError, ct).ConfigureAwait(false);
            }
        }
    }

    private void HandleMessage(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        string? messageType = root.TryGetProperty("type", out JsonElement typeElement)
            ? typeElement.GetString()
            : null;
        string? requestId = root.TryGetProperty("requestId", out JsonElement requestIdElement)
            ? requestIdElement.GetString()
            : null;
        string? operation = root.TryGetProperty("op", out JsonElement operationElement)
            ? operationElement.GetString()
            : null;

        switch (messageType)
        {
            case "invokeResult":
                if (requestId is not null && _pending.TryRemove(requestId, out TaskCompletionSource<object?>? completion))
                {
                    if (root.TryGetProperty("error", out JsonElement errorElement) &&
                        errorElement.ValueKind is not JsonValueKind.Null and not JsonValueKind.Undefined)
                    {
                        string errorMessage = errorElement.ValueKind == JsonValueKind.String
                            ? errorElement.GetString() ?? "Remote operation failed."
                            : errorElement.GetRawText();
                        Exception exception = new InvalidOperationException(errorMessage);
                        if (operation == "SubscribeSignals")
                        {
                            OnRegisterError?.Invoke(
                                _uri.ToString(),
                                exception,
                                -1,
                                Array.Empty<int>(),
                                Array.Empty<string>());
                        }
                        completion.TrySetException(exception);
                    }
                    else
                    {
                        object? result = root.TryGetProperty("payload", out JsonElement payload)
                            ? payload.GetRawText()
                            : null;
                        completion.TrySetResult(result);
                    }
                }
                break;

            case "streamItem":
                if (!root.TryGetProperty("payload", out JsonElement streamPayload))
                {
                    throw new JsonException("A stream item did not contain a payload.");
                }

                if (streamPayload.TryGetProperty("errorCode", out JsonElement errorCodes) &&
                    streamPayload.TryGetProperty("signalNames", out JsonElement signalNames))
                {
                    OnRegisterError?.Invoke(
                        _uri.ToString(),
                        new InvalidOperationException("Signal registration failed."),
                        -1,
                        errorCodes.EnumerateArray().Select(value => value.GetInt32()).ToArray(),
                        signalNames.EnumerateArray().Select(value => value.GetString() ?? string.Empty).ToArray());
                }
                _streamItems.Writer.TryWrite(streamPayload.GetRawText());
                break;
        }
    }

    private async Task HandleConnectionClosedAsync(Exception? connectionError, CancellationToken ct)
    {
        Exception error = connectionError ?? new WebSocketException("The WebSocket connection was closed.");
        OnCommunicationError?.Invoke(_uri.ToString(), error, -1);

        foreach (KeyValuePair<string, TaskCompletionSource<object?>> pendingRequest in _pending)
        {
            pendingRequest.Value.TrySetException(
                new WebSocketException("The connection closed before the request completed.", error));
        }
        _pending.Clear();

        while (!ct.IsCancellationRequested && Volatile.Read(ref _disposed) == 0)
        {
            try
            {
                await Task.Delay(_reconnectDelay, ct).ConfigureAwait(false);
                await ConnectAsync(ct).ConfigureAwait(false);
                return;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                return;
            }
            catch (WebSocketException exception)
            {
                OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
            }
            catch (HttpRequestException exception)
            {
                OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
            }
        }
    }

    private async Task NotifyReconnectedAsync()
    {
        Func<Task>? handlers = OnReconnected;
        if (handlers is null)
        {
            return;
        }

        foreach (Delegate registeredHandler in handlers.GetInvocationList())
        {
            try
            {
                await ((Func<Task>)registeredHandler)().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                OnCommunicationError?.Invoke(_uri.ToString(), exception, -1);
            }
        }
    }

    private async Task ProcessStreamItemsAsync(CancellationToken ct)
    {
        try
        {
            await foreach (object? item in _streamItems.Reader.ReadAllAsync(ct).ConfigureAwait(false))
            {
                if (item is null)
                {
                    continue;
                }

                Action<string, object>? handlers = OnStreamItemReceived;
                if (handlers is null)
                {
                    continue;
                }

                foreach (Delegate registeredHandler in handlers.GetInvocationList())
                {
                    try
                    {
                        ((Action<string, object>)registeredHandler)(_uri.ToString(), item);
                    }
                    catch (Exception exception)
                    {
                        OnCommunicationError?.Invoke(_uri.ToString(), exception, -2);
                    }
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        ClientWebSocket socket = _webSocket;
        try
        {
            if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "dispose",
                    CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch (WebSocketException)
        {
        }

        _streamItems.Writer.TryComplete();
        if (_receiveLoopTask is not null)
        {
            await _receiveLoopTask.ConfigureAwait(false);
        }
        await _streamDispatchTask.ConfigureAwait(false);
        socket.Dispose();
        _connectionLock.Dispose();
        _sendLock.Dispose();
        _disposeTokenSource.Dispose();
    }
}
