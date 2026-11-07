// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAULine1 : HMIISABase
{
    static HMIISAULine1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAULine1),
            new FrameworkPropertyMetadata(typeof(HMIISAULine1)));
    }
}
