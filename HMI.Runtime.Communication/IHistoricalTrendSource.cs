// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public sealed record HistoricalTrendPoint(DateTime Timestamp, double Value, int Status);

public sealed record HistoricalTrendSeries(
    string SignalName,
    IReadOnlyList<HistoricalTrendPoint> Points);

public interface IHistoricalTrendSource
{
    Task<IReadOnlyList<HistoricalTrendSeries>> GetHistoricalTrendsAsync(
        IReadOnlyList<string> signalNames,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int archiveBit = 0,
        CancellationToken cancellationToken = default);
}
