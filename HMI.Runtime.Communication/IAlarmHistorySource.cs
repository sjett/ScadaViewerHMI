// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public sealed record AlarmHistoryEvent(
    Guid Id,
    string ChannelName,
    string Message,
    int Severity,
    DateTime Timestamp,
    bool Acknowledged,
    DateTime? AcknowledgedAt,
    string AlarmGroupName = "",
    string AlarmTypeName = "");

public interface IAlarmHistorySource
{
    Task<IReadOnlyList<AlarmHistoryEvent>> GetAlarmHistoryAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);
}
