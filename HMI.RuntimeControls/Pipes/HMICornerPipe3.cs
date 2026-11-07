// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMICornerPipe3 : PipeControl
{
    static HMICornerPipe3()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerPipe3), new FrameworkPropertyMetadata(typeof(HMICornerPipe3)));
    }
}
