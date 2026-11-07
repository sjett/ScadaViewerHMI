// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIAnimatedTriangle1 : HMIAnimatedPrimitiveBase
{
    static HMIAnimatedTriangle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedTriangle1), new FrameworkPropertyMetadata(typeof(HMIAnimatedTriangle1)));
    }
}
