// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeatExchanger4 : HMIISABase
{
    static HMIISAHeatExchanger4()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeatExchanger4),
            new FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger4)));
    }
}
