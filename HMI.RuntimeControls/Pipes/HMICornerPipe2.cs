// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMICornerPipe2 : PipeControl
{
    static HMICornerPipe2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerPipe2), new FrameworkPropertyMetadata(typeof(HMICornerPipe2)));
    }
}
