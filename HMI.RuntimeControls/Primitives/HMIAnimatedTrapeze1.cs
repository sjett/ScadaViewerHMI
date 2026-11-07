// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIAnimatedTrapeze1 : HMIAnimatedPrimitiveBase
{
    static HMIAnimatedTrapeze1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedTrapeze1), new FrameworkPropertyMetadata(typeof(HMIAnimatedTrapeze1)));
    }
}
