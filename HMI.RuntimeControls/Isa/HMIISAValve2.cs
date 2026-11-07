// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValve2 : HMIISABase
{
    static HMIISAValve2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValve2),
            new FrameworkPropertyMetadata(typeof(HMIISAValve2)));
    }
}
