// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMITPipe1 : PipeControl
{
    static HMITPipe1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITPipe1), new FrameworkPropertyMetadata(typeof(HMITPipe1)));
    }
}
