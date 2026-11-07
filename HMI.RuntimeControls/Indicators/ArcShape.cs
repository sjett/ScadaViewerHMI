// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HMI.RuntimeControls.Indicators;
public class ArcShape : Shape
{
    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(ArcShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty EndAngleProperty =
        DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(ArcShape),
            new FrameworkPropertyMetadata(90.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius), typeof(double), typeof(ArcShape),
            new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ArcThicknessProperty =
        DependencyProperty.Register(nameof(ArcThickness), typeof(double), typeof(ArcShape),
            new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double StartAngle
    {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public double EndAngle
    {
        get => (double)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public double ArcThickness
    {
        get => (double)GetValue(ArcThicknessProperty);
        set => SetValue(ArcThicknessProperty, value);
    }

    protected override Geometry DefiningGeometry
    {
        get
        {
            var outerRadius = Radius;
            var innerRadius = Radius - ArcThickness;

            var startRadians = StartAngle * Math.PI / 180;
            var endRadians = EndAngle * Math.PI / 180;

            // 外弧起点和终点
            var outerStart = new Point(
                outerRadius + outerRadius * Math.Cos(startRadians),
                outerRadius + outerRadius * Math.Sin(startRadians));

            var outerEnd = new Point(
                outerRadius + outerRadius * Math.Cos(endRadians),
                outerRadius + outerRadius * Math.Sin(endRadians));

            // 内弧起点和终点
            var innerStart = new Point(
                outerRadius + innerRadius * Math.Cos(endRadians),
                outerRadius + innerRadius * Math.Sin(endRadians));

            var innerEnd = new Point(
                outerRadius + innerRadius * Math.Cos(startRadians),
                outerRadius + innerRadius * Math.Sin(startRadians));

            var isLargeArc = Math.Abs(EndAngle - StartAngle) > 180;

            var figure = new PathFigure { StartPoint = outerStart, IsClosed = true };

            // 外弧
            figure.Segments.Add(new ArcSegment
            {
                Point = outerEnd,
                Size = new Size(outerRadius, outerRadius),
                IsLargeArc = isLargeArc,
                SweepDirection = SweepDirection.Clockwise
            });

            // 连线到内弧起点
            figure.Segments.Add(new LineSegment(innerStart, true));

            // 内弧（反向）
            figure.Segments.Add(new ArcSegment
            {
                Point = innerEnd,
                Size = new Size(innerRadius, innerRadius),
                IsLargeArc = isLargeArc,
                SweepDirection = SweepDirection.Counterclockwise
            });

            // 闭合
            figure.Segments.Add(new LineSegment(outerStart, true));

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            return geometry;
        }
    }
}
