// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACooling2 : HMIISABase
{
    static HMIISACooling2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACooling2),
            new FrameworkPropertyMetadata(typeof(HMIISACooling2)));
    }
}
