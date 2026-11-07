// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveBall1 : HMIISABase
{
    static HMIISAValveBall1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveBall1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveBall1)));
    }
}
