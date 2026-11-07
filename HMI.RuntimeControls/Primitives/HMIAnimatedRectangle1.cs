// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIAnimatedRectangle1 : HMIAnimatedPrimitiveBase
{
    static HMIAnimatedRectangle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedRectangle1), new FrameworkPropertyMetadata(typeof(HMIAnimatedRectangle1)));
    }
}
