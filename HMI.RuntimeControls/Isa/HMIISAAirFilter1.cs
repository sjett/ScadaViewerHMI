// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAAirFilter1 : HMIISABase
{
    static HMIISAAirFilter1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAAirFilter1),
            new FrameworkPropertyMetadata(typeof(HMIISAAirFilter1)));
    }
}
