// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValve3Way1 : HMIISABase
{
    static HMIISAValve3Way1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValve3Way1),
            new FrameworkPropertyMetadata(typeof(HMIISAValve3Way1)));
    }
}
