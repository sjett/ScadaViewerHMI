// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFlap3 : HMIISABase
{
    static HMIISAFlap3()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFlap3),
            new FrameworkPropertyMetadata(typeof(HMIISAFlap3)));
    }
}
