// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAFunnel1 : HMIISABase
{
    static HMIISAFunnel1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAFunnel1),
            new FrameworkPropertyMetadata(typeof(HMIISAFunnel1)));
    }
}
