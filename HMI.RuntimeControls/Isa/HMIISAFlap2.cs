// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFlap2 : HMIISABase
{
    static HMIISAFlap2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFlap2),
            new FrameworkPropertyMetadata(typeof(HMIISAFlap2)));
    }
}
