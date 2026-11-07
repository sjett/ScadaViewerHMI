// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public sealed record AlarmFilterMetadata(
    IReadOnlyList<string> GroupNames,
    IReadOnlyList<string> TypeNames);

public interface IAlarmMetadataSource
{
    Task<AlarmFilterMetadata> GetAlarmFilterMetadataAsync(CancellationToken cancellationToken = default);
}
