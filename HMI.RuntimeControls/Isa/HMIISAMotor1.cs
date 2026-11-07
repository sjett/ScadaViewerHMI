// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAMotor1 : HMIISABase
{
    static HMIISAMotor1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAMotor1),
            new FrameworkPropertyMetadata(typeof(HMIISAMotor1)));
    }
}
