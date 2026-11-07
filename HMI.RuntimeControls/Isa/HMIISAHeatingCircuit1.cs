// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAHeatingCircuit1 : HMIISABase
{
    static HMIISAHeatingCircuit1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAHeatingCircuit1),
            new FrameworkPropertyMetadata(typeof(HMIISAHeatingCircuit1)));
    }
}
