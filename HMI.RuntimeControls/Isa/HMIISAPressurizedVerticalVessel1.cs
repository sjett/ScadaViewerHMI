// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressurizedVerticalVessel1 : HMIISABase
{
    static HMIISAPressurizedVerticalVessel1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressurizedVerticalVessel1),
            new FrameworkPropertyMetadata(typeof(HMIISAPressurizedVerticalVessel1)));
    }
}
