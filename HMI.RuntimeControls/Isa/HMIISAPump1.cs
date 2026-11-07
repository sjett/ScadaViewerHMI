// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPump1 : HMIISABase
{
    static HMIISAPump1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPump1),
            new FrameworkPropertyMetadata(typeof(HMIISAPump1)));
    }
}
