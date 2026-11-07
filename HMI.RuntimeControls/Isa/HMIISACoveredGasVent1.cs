// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACoveredGasVent1 : HMIISABase
{
    static HMIISACoveredGasVent1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACoveredGasVent1),
            new FrameworkPropertyMetadata(typeof(HMIISACoveredGasVent1)));
    }
}
