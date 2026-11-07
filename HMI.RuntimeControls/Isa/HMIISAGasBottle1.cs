// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAGasBottle1 : HMIISABase
{
    static HMIISAGasBottle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAGasBottle1),
            new FrameworkPropertyMetadata(typeof(HMIISAGasBottle1)));
    }
}
