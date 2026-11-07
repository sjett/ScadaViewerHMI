// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPackingColumn1 : HMIISABase
{
    static HMIISAPackingColumn1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPackingColumn1),
            new FrameworkPropertyMetadata(typeof(HMIISAPackingColumn1)));
    }
}
