// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeatExchanger3 : HMIISABase
{
    static HMIISAHeatExchanger3()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeatExchanger3),
            new FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger3)));
    }
}
