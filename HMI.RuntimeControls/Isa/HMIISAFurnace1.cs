// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFurnace1 : HMIISABase
{
    static HMIISAFurnace1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFurnace1),
            new FrameworkPropertyMetadata(typeof(HMIISAFurnace1)));
    }
}
