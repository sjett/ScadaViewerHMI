// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMITPipe3 : PipeControl
{
    static HMITPipe3()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITPipe3), new FrameworkPropertyMetadata(typeof(HMITPipe3)));
    }
}
