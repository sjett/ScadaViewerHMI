// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveDiaphragm1 : HMIISABase
{
    static HMIISAValveDiaphragm1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveDiaphragm1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveDiaphragm1)));
    }
}
