// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveAngle1 : HMIISABase
{
    static HMIISAValveAngle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveAngle1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveAngle1)));
    }
}
