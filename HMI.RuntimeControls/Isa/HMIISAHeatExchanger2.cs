// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeatExchanger2 : HMIISABase
{
    static HMIISAHeatExchanger2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeatExchanger2),
            new FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger2)));
    }
}
