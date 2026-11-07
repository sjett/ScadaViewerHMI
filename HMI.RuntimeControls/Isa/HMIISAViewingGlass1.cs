// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAViewingGlass1 : HMIISABase
{
    static HMIISAViewingGlass1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAViewingGlass1),
            new FrameworkPropertyMetadata(typeof(HMIISAViewingGlass1)));
    }
}
