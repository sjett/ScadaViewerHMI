// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveBlowDownAngle1 : HMIISABase
{
    static HMIISAValveBlowDownAngle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveBlowDownAngle1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveBlowDownAngle1)));
    }
}
