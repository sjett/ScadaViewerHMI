// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISATrayColumn1 : HMIISABase
{
    static HMIISATrayColumn1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISATrayColumn1),
            new FrameworkPropertyMetadata(typeof(HMIISATrayColumn1)));
    }
}
