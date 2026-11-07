// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeating2 : HMIISABase
{
    static HMIISAHeating2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeating2),
            new FrameworkPropertyMetadata(typeof(HMIISAHeating2)));
    }
}
