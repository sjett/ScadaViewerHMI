// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAShellAndTubeHeatExchanger1 : HMIISABase
{
    static HMIISAShellAndTubeHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAShellAndTubeHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISAShellAndTubeHeatExchanger1)));
    }
}
