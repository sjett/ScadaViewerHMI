// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFilter1 : HMIISABase
{
    static HMIISAFilter1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFilter1),
            new FrameworkPropertyMetadata(typeof(HMIISAFilter1)));
    }
}
