// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIEllipse1 : HMIPrimitiveBase
{
    static HMIEllipse1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIEllipse1), new FrameworkPropertyMetadata(typeof(HMIEllipse1)));
    }
}
