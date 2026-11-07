// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISASpiralHeatExchanger1 : HMIISABase
{
    static HMIISASpiralHeatExchanger1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISASpiralHeatExchanger1),
            new FrameworkPropertyMetadata(typeof(HMIISASpiralHeatExchanger1)));
    }
}
