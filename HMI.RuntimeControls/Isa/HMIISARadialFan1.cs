// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARadialFan1 : HMIISABase
{
    static HMIISARadialFan1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARadialFan1),
            new FrameworkPropertyMetadata(typeof(HMIISARadialFan1)));
    }
}
