// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAAxialFan1 : HMIISABase
{
    static HMIISAAxialFan1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAAxialFan1),
            new FrameworkPropertyMetadata(typeof(HMIISAAxialFan1)));
    }
}
