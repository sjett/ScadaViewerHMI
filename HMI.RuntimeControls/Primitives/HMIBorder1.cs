// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIBorder1 : System.Windows.Controls.Border
{
    static HMIBorder1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBorder1), new FrameworkPropertyMetadata(typeof(HMIBorder1)));
    }
}
