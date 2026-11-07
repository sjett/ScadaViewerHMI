// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveDiaphragmA1 : HMIISABase
{
    static HMIISAValveDiaphragmA1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveDiaphragmA1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveDiaphragmA1)));
    }
}
