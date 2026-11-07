// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public sealed class HmiApiConnectionOptions
{
    public required string Name { get; init; }
    public required string Host { get; init; }
    public int Port { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Instance { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int Timeout { get; init; } = 10000;
    public int SubscriptionPollingIntervalMilliseconds { get; init; } = 500;
}

public interface IHmiApiClientFactory
{
    IHmiApiClient Create(HmiApiConnectionOptions options);
}

public sealed class RapidScadaHmiApiClientFactory : IHmiApiClientFactory
{
    public IHmiApiClient Create(HmiApiConnectionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new RapidScadaHmiApiClient(options);
    }
}
