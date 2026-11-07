// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMITLine1 : PipeControl
{
    static HMITLine1() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITLine1), new FrameworkPropertyMetadata(typeof(HMITLine1)));
}
