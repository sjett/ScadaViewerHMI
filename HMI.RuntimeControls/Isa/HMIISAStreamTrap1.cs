// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAStreamTrap1 : HMIISABase
{
    static HMIISAStreamTrap1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAStreamTrap1),
            new FrameworkPropertyMetadata(typeof(HMIISAStreamTrap1)));
    }
}
