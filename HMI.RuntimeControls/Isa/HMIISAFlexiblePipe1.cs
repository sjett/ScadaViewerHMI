// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFlexiblePipe1 : HMIISABase
{
    static HMIISAFlexiblePipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFlexiblePipe1),
            new FrameworkPropertyMetadata(typeof(HMIISAFlexiblePipe1)));
    }
}
