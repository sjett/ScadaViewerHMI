// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACornerLine1 : HMIISABase
{
    static HMIISACornerLine1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACornerLine1),
            new FrameworkPropertyMetadata(typeof(HMIISACornerLine1)));
    }
}
