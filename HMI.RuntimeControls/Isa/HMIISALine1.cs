// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISALine1 : HMIISABase
{
    static HMIISALine1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISALine1),
            new FrameworkPropertyMetadata(typeof(HMIISALine1)));
    }
}
