// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARegulatorDiscPressure1 : HMIISABase
{
    static HMIISARegulatorDiscPressure1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARegulatorDiscPressure1),
            new FrameworkPropertyMetadata(typeof(HMIISARegulatorDiscPressure1)));
    }
}
