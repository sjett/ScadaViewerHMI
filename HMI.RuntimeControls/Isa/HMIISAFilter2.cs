// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFilter2 : HMIISABase
{
    static HMIISAFilter2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFilter2),
            new FrameworkPropertyMetadata(typeof(HMIISAFilter2)));
    }
}
