// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPlateHeatExchanger1 : HMIISABase
{
    static HMIISAPlateHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPlateHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISAPlateHeatExchanger1)));
    }
}
