// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveBleedPurge1 : HMIISABase
{
    static HMIISAValveBleedPurge1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveBleedPurge1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveBleedPurge1)));
    }
}
