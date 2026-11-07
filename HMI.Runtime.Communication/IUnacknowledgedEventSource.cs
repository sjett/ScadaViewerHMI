// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

/// <summary>An event requiring acknowledgement; it does not indicate the current alarm state.</summary>
public sealed record UnacknowledgedEvent(
    Guid Id,
    string ChannelName,
    string Message,
    int Severity,
    DateTime Timestamp);

public interface IUnacknowledgedEventSource
{
    /// <summary>
    /// Gets unacknowledged events from the specified inclusive time range.
    /// Events outside this range are not included.
    /// </summary>
    Task<IReadOnlyList<UnacknowledgedEvent>> GetUnacknowledgedEventsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);
}
