// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMICornerPipe1 : PipeControl
{
    static HMICornerPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerPipe1), new FrameworkPropertyMetadata(typeof(HMICornerPipe1)));
    }
}
