// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISA2WayValve2 : HMIISABase
{
    static HMIISA2WayValve2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISA2WayValve2),
            new FrameworkPropertyMetadata(typeof(HMIISA2WayValve2)));
    }
}
