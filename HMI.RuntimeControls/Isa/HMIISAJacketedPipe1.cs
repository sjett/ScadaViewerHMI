// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAJacketedPipe1 : HMIISABase
{
    static HMIISAJacketedPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAJacketedPipe1),
            new FrameworkPropertyMetadata(typeof(HMIISAJacketedPipe1)));
    }
}
