// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAUShapedTubesHeatExchanger1 : HMIISABase
{
    static HMIISAUShapedTubesHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAUShapedTubesHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISAUShapedTubesHeatExchanger1)));
    }
}
