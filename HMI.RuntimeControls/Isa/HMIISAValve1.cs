// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValve1 : HMIISABase
{
    static HMIISAValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValve1),
            new FrameworkPropertyMetadata(typeof(HMIISAValve1)));
    }
}
