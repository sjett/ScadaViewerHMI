// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public interface ISignalCatalogSource
{
    Task<IReadOnlyList<string>> GetSignalNamesAsync(CancellationToken cancellationToken = default);
}
