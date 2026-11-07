// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMILinePipe1 : PipeControl
{
    static HMILinePipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILinePipe1), new FrameworkPropertyMetadata(typeof(HMILinePipe1)));
    }
}
