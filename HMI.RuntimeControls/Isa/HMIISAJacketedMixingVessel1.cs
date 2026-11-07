// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAJacketedMixingVessel1 : HMIISABase
{
    static HMIISAJacketedMixingVessel1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAJacketedMixingVessel1),
            new FrameworkPropertyMetadata(typeof(HMIISAJacketedMixingVessel1)));
    }
}
