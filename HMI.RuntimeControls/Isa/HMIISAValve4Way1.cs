// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValve4Way1 : HMIISABase
{
    static HMIISAValve4Way1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValve4Way1),
            new FrameworkPropertyMetadata(typeof(HMIISAValve4Way1)));
    }
}
