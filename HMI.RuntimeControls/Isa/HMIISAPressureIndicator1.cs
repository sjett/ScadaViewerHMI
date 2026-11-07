// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressureIndicator1 : HMIISABase
{
    static HMIISAPressureIndicator1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressureIndicator1),
            new FrameworkPropertyMetadata(typeof(HMIISAPressureIndicator1)));
    }
}
