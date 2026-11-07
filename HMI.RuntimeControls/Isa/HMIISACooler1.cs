// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACooler1 : HMIISABase
{
    static HMIISACooler1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACooler1),
            new FrameworkPropertyMetadata(typeof(HMIISACooler1)));
    }
}
