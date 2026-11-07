// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Pipes;

public sealed class HMICrossLine1 : PipeControl
{
    static HMICrossLine1() => DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICrossLine1), new FrameworkPropertyMetadata(typeof(HMICrossLine1)));
}
