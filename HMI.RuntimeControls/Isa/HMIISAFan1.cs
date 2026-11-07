// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFan1 : HMIISABase
{
    static HMIISAFan1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFan1),
            new FrameworkPropertyMetadata(typeof(HMIISAFan1)));
    }
}
