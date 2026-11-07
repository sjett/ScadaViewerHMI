// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAValveCylinderActuated1 : HMIISABase
{
    static HMIISAValveCylinderActuated1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAValveCylinderActuated1),
            new FrameworkPropertyMetadata(typeof(HMIISAValveCylinderActuated1)));
    }
}
