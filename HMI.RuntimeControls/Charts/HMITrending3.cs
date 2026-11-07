// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Charts;

public sealed class HMITrending3 : HMITrending2
{
    static HMITrending3()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMITrending3),
            new FrameworkPropertyMetadata(typeof(HMITrending3)));
    }
}
