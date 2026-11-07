// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISATLine1 : HMIISABase
{
    static HMIISATLine1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISATLine1),
            new FrameworkPropertyMetadata(typeof(HMIISATLine1)));
    }
}
