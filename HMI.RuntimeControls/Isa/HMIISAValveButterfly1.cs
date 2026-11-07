// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveButterfly1 : HMIISABase
{
    static HMIISAValveButterfly1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveButterfly1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveButterfly1)));
    }
}
