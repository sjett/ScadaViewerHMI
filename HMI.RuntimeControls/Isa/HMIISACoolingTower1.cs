// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACoolingTower1 : HMIISABase
{
    static HMIISACoolingTower1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACoolingTower1),
            new FrameworkPropertyMetadata(typeof(HMIISACoolingTower1)));
    }
}
