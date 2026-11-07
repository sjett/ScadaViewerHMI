// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISANeedleValve1 : HMIISABase
{
    static HMIISANeedleValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISANeedleValve1),
            new FrameworkPropertyMetadata(typeof(HMIISANeedleValve1)));
    }
}
