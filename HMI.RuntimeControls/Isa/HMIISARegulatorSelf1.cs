// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARegulatorSelf1 : HMIISABase
{
    static HMIISARegulatorSelf1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARegulatorSelf1),
            new FrameworkPropertyMetadata(typeof(HMIISARegulatorSelf1)));
    }
}
