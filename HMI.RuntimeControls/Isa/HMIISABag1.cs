// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISABag1 : HMIISABase
{
    static HMIISABag1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISABag1),
            new FrameworkPropertyMetadata(typeof(HMIISABag1)));
    }
}
