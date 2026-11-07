// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISACooledOrHeatedPipe1 : HMIISABase
{
    static HMIISACooledOrHeatedPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISACooledOrHeatedPipe1),
            new FrameworkPropertyMetadata(typeof(HMIISACooledOrHeatedPipe1)));
    }
}
