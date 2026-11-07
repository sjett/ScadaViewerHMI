// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPump2 : HMIISABase
{
    static HMIISAPump2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPump2),
            new FrameworkPropertyMetadata(typeof(HMIISAPump2)));
    }
}
