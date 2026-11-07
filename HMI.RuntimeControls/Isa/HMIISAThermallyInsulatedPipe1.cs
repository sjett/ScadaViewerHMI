// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAThermallyInsulatedPipe1 : HMIISABase
{
    static HMIISAThermallyInsulatedPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAThermallyInsulatedPipe1),
            new FrameworkPropertyMetadata(typeof(HMIISAThermallyInsulatedPipe1)));
    }
}
