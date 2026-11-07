// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveCheck1 : HMIISABase
{
    static HMIISAValveCheck1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveCheck1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveCheck1)));
    }
}
