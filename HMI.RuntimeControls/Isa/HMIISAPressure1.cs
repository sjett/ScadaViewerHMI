// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressure1 : HMIISABase
{
    static HMIISAPressure1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressure1),
            new FrameworkPropertyMetadata(typeof(HMIISAPressure1)));
    }
}
