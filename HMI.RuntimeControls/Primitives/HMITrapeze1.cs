// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMITrapeze1 : HMIPrimitiveBase
{
    static HMITrapeze1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITrapeze1), new FrameworkPropertyMetadata(typeof(HMITrapeze1)));
    }
}
