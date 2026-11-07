// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISA3WayValve2 : HMIISABase
{
    static HMIISA3WayValve2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISA3WayValve2),
            new FrameworkPropertyMetadata(typeof(HMIISA3WayValve2)));
    }
}
