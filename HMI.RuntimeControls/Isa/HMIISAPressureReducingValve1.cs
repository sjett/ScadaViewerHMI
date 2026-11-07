// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPressureReducingValve1 : HMIISABase
{
    static HMIISAPressureReducingValve1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPressureReducingValve1),
            new FrameworkPropertyMetadata(typeof(HMIISAPressureReducingValve1)));
    }
}
