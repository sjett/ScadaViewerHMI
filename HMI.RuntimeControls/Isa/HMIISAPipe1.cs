// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISAPipe1 : HMIISABase
{
    static HMIISAPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISAPipe1),
            new FrameworkPropertyMetadata(typeof(HMIISAPipe1)));
    }
}
