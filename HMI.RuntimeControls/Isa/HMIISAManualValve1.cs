// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAManualValve1 : HMIISABase
{
    static HMIISAManualValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAManualValve1),
            new FrameworkPropertyMetadata(typeof(HMIISAManualValve1)));
    }
}
