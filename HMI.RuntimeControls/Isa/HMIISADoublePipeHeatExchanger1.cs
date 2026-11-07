// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISADoublePipeHeatExchanger1 : HMIISABase
{
    static HMIISADoublePipeHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISADoublePipeHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISADoublePipeHeatExchanger1)));
    }
}
