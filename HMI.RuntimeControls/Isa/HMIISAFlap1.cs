// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFlap1 : HMIISABase
{
    static HMIISAFlap1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFlap1),
            new FrameworkPropertyMetadata(typeof(HMIISAFlap1)));
    }
}
