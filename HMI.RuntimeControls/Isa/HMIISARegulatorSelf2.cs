// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARegulatorSelf2 : HMIISABase
{
    static HMIISARegulatorSelf2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARegulatorSelf2),
            new FrameworkPropertyMetadata(typeof(HMIISARegulatorSelf2)));
    }
}
