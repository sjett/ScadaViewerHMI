// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISARuptureDiscVacuum1 : HMIISABase
{
    static HMIISARuptureDiscVacuum1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISARuptureDiscVacuum1),
            new FrameworkPropertyMetadata(typeof(HMIISARuptureDiscVacuum1)));
    }
}
