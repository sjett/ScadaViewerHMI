// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISADryer1 : HMIISABase
{
    static HMIISADryer1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISADryer1),
            new FrameworkPropertyMetadata(typeof(HMIISADryer1)));
    }
}
