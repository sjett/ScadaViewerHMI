// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISADiaphragmValve1 : HMIISABase
{
    static HMIISADiaphragmValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISADiaphragmValve1),
            new FrameworkPropertyMetadata(typeof(HMIISADiaphragmValve1)));
    }
}
