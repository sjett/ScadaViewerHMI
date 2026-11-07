// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveCheckStop1 : HMIISABase
{
    static HMIISAValveCheckStop1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveCheckStop1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveCheckStop1)));
    }
}
