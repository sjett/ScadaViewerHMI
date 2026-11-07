// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMILine1 : PipeControl
{
    static HMILine1() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILine1), new FrameworkPropertyMetadata(typeof(HMILine1)));
}
