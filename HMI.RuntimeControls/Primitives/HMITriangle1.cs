// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMITriangle1 : HMIPrimitiveBase
{
    static HMITriangle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITriangle1), new FrameworkPropertyMetadata(typeof(HMITriangle1)));
    }
}
