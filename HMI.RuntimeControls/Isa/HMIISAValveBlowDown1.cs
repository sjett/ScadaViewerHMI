// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveBlowDown1 : HMIISABase
{
    static HMIISAValveBlowDown1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveBlowDown1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveBlowDown1)));
    }
}
