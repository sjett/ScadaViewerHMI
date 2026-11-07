// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressurizedHorizontalVessel1 : HMIISABase
{
    static HMIISAPressurizedHorizontalVessel1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressurizedHorizontalVessel1),
            new FrameworkPropertyMetadata(typeof(HMIISAPressurizedHorizontalVessel1)));
    }
}
