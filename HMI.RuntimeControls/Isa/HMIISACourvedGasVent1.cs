// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACourvedGasVent1 : HMIISABase
{
    static HMIISACourvedGasVent1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACourvedGasVent1),
            new FrameworkPropertyMetadata(typeof(HMIISACourvedGasVent1)));
    }
}
