// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARegulatorExtPressValve1 : HMIISABase
{
    static HMIISARegulatorExtPressValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARegulatorExtPressValve1),
            new FrameworkPropertyMetadata(typeof(HMIISARegulatorExtPressValve1)));
    }
}
