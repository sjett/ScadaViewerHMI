// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public sealed record OperationEvent(
    long Id,
    DateTime Timestamp,
    string SignalName,
    string Text,
    double? PreviousValue,
    double? Value,
    int Severity,
    bool AcknowledgementRequired,
    bool Acknowledged,
    DateTime? AcknowledgedAt,
    int DeviceNumber,
    int ObjectNumber)
{
    public DateTime Date => Timestamp.Date;
}

public interface IOperationEventSource
{
    Task<IReadOnlyList<OperationEvent>> GetOperationEventsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int maxEvents,
        CancellationToken cancellationToken = default);
}
