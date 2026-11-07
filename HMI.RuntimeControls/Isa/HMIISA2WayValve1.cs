// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISA2WayValve1 : HMIISABase
{
    static HMIISA2WayValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISA2WayValve1),
            new FrameworkPropertyMetadata(typeof(HMIISA2WayValve1)));
    }
}
