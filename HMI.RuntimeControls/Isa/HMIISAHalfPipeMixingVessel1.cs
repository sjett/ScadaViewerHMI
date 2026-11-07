// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHalfPipeMixingVessel1 : HMIISABase
{
    static HMIISAHalfPipeMixingVessel1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHalfPipeMixingVessel1),
            new FrameworkPropertyMetadata(typeof(HMIISAHalfPipeMixingVessel1)));
    }
}
