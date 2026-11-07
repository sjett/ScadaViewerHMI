// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACrossLine1 : HMIISABase
{
    static HMIISACrossLine1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACrossLine1),
            new FrameworkPropertyMetadata(typeof(HMIISACrossLine1)));
    }
}
