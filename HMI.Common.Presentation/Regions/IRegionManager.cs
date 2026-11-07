// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Regions;

public interface IRegionManager
{
    void RegisterViewWithRegion(string regionName, Type viewType);
}
