// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMICornerLine1 : PipeControl
{
    static HMICornerLine1() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerLine1), new FrameworkPropertyMetadata(typeof(HMICornerLine1)));
}
