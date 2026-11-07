// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIAnimatedEllipse1 : HMIAnimatedPrimitiveBase
{
    static HMIAnimatedEllipse1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedEllipse1), new FrameworkPropertyMetadata(typeof(HMIAnimatedEllipse1)));
    }
}
