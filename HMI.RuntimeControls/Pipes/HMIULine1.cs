// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMIULine1 : PipeControl
{
    static HMIULine1() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIULine1), new FrameworkPropertyMetadata(typeof(HMIULine1)));
}
