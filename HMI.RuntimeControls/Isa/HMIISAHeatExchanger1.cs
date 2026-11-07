// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeatExchanger1 : HMIISABase
{
    static HMIISAHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger1)));
    }
}
