// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Primitives;

public sealed class HMIRectangle1 : HMIPrimitiveBase
{
    public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(
        nameof(RadiusX), typeof(double), typeof(HMIRectangle1), new PropertyMetadata(0d));
    public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(
        nameof(RadiusY), typeof(double), typeof(HMIRectangle1), new PropertyMetadata(0d));

    public double RadiusX { get => (double)GetValue(RadiusXProperty); set => SetValue(RadiusXProperty, value); }
    public double RadiusY { get => (double)GetValue(RadiusYProperty); set => SetValue(RadiusYProperty, value); }

    static HMIRectangle1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIRectangle1), new FrameworkPropertyMetadata(typeof(HMIRectangle1)));
    }
}
