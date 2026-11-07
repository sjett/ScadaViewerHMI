// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAVentilator1 : HMIISABase
{
    static HMIISAVentilator1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAVentilator1),
            new FrameworkPropertyMetadata(typeof(HMIISAVentilator1)));
    }
}
