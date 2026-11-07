// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressure2 : HMIISABase
{
    static HMIISAPressure2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressure2),
            new FrameworkPropertyMetadata(typeof(HMIISAPressure2)));
    }
}
