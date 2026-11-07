// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMITPipe2 : PipeControl
{
    static HMITPipe2()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITPipe2), new FrameworkPropertyMetadata(typeof(HMITPipe2)));
    }
}
