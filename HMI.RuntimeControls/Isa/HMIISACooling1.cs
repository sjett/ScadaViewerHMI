// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACooling1 : HMIISABase
{
    static HMIISACooling1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACooling1),
            new FrameworkPropertyMetadata(typeof(HMIISACooling1)));
    }
}
