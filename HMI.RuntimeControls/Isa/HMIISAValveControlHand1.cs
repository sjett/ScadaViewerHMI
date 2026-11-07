// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveControlHand1 : HMIISABase
{
    static HMIISAValveControlHand1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveControlHand1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveControlHand1)));
    }
}
