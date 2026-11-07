// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HMI.RuntimeControls.Indicators;

    /// <summary>
    /// 沿路径布局子元素的列表控件（支持 SourceElement 绑定）
    /// </summary>
    public class PathListBox : ListBox
    {
        static PathListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathListBox),
                new FrameworkPropertyMetadata(typeof(PathListBox)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty LayoutPathProperty =
            DependencyProperty.Register(nameof(LayoutPath), typeof(Geometry),
                typeof(PathListBox), new PropertyMetadata(null, OnLayoutPathChanged));

        public static readonly DependencyProperty SourceElementProperty =
            DependencyProperty.Register(nameof(SourceElement), typeof(object),
                typeof(PathListBox), new PropertyMetadata(null, OnSourceElementChanged));

        public static readonly DependencyProperty PathSourceTypeProperty =
            DependencyProperty.Register(nameof(PathSourceType), typeof(PathSourceType),
                typeof(PathListBox), new PropertyMetadata(PathSourceType.Geometry, OnSourceElementChanged));

        public static readonly DependencyProperty DistributionProperty =
            DependencyProperty.Register(nameof(Distribution), typeof(PathDistribution),
                typeof(PathListBox), new PropertyMetadata(PathDistribution.Even, OnLayoutPathChanged));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(PathOrientation),
                typeof(PathListBox), new PropertyMetadata(PathOrientation.None, OnLayoutPathChanged));

        public static readonly DependencyProperty StartOffsetProperty =
            DependencyProperty.Register(nameof(StartOffset), typeof(double),
                typeof(PathListBox), new PropertyMetadata(0.0, OnLayoutPathChanged));

        public static readonly DependencyProperty PathSpacingProperty =
            DependencyProperty.Register(nameof(PathSpacing), typeof(double),
                typeof(PathListBox), new PropertyMetadata(0.0, OnLayoutPathChanged));

        #endregion

        #region CLR Properties

        public Geometry LayoutPath
        {
            get => (Geometry)GetValue(LayoutPathProperty);
            set => SetValue(LayoutPathProperty, value);
        }

        public object SourceElement
        {
            get => GetValue(SourceElementProperty);
            set => SetValue(SourceElementProperty, value);
        }

        public PathSourceType PathSourceType
        {
            get => (PathSourceType)GetValue(PathSourceTypeProperty);
            set => SetValue(PathSourceTypeProperty, value);
        }

        public PathDistribution Distribution
        {
            get => (PathDistribution)GetValue(DistributionProperty);
            set => SetValue(DistributionProperty, value);
        }

        public PathOrientation Orientation
        {
            get => (PathOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double StartOffset
        {
            get => (double)GetValue(StartOffsetProperty);
            set => SetValue(StartOffsetProperty, Math.Max(0, Math.Min(1, value)));
        }

        public double PathSpacing
        {
            get => (double)GetValue(PathSpacingProperty);
            set => SetValue(PathSpacingProperty, Math.Max(0, value));
        }

        #endregion

        #region Private Fields

        private PathListBoxPanel? _panel;
        private FrameworkElement? _currentSourceElement;
        private Geometry? _currentGeometrySource;
        private PropertyChangedEventHandler? _sourceElementHandler;
    #endregion

    #region Property Changed Handlers

    private static void OnLayoutPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PathListBox)?.UpdatePanelLayout();
        }

    private static void OnSourceElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var pathListBox = d as PathListBox;
        var oldElement = e.OldValue;
        var newElement = e.NewValue;
        pathListBox?.OnSourceElementChanged(oldElement, newElement);
    }

    private void OnSourceElementChanged(object? oldElement, object? newElement)
    {
        // 移除旧元素的监听
        if (oldElement is FrameworkElement oldFe)
        {
            UnsubscribeFromElement(oldFe);
        }

        _currentSourceElement = null;
        _currentGeometrySource = null;

        if (newElement is FrameworkElement fe)
        {
            _currentSourceElement = fe;
            SubscribeToElement(fe);
        }
        else if (newElement is Geometry geometry)
        {
            _currentGeometrySource = geometry;
        }

        // 立即更新一次
        UpdateLayoutFromSourceElement();
    }
    private void UpdateLayoutFromSourceElement()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            Geometry? geometry = null;

            if (_currentGeometrySource != null)
            {
                geometry = _currentGeometrySource;
            }
            else if (_currentSourceElement != null)
            {
                geometry = ExtractGeometryFromSource(_currentSourceElement);
            }

            if (geometry != null && !geometry.IsEmpty())
            {
                LayoutPath = geometry;
            }
        }), System.Windows.Threading.DispatcherPriority.Render);
    }

    private Geometry? ExtractGeometryFromSource(FrameworkElement source)
    {
        switch (PathSourceType)
        {
            case PathSourceType.Geometry:
                return ExtractGeometry(source);

            case PathSourceType.ActualShape:
                return ExtractActualShapeGeometry(source);

            case PathSourceType.RenderBounds:
                return ExtractRenderBoundsGeometry(source);

            case PathSourceType.VisualTree:
                return ExtractVisualTreeGeometry(source);

            default:
                return ExtractGeometry(source);
        }
    }

    private Geometry? ExtractGeometry(FrameworkElement source)
    {
        // 如果是 Shape，获取其 RenderedGeometry
        if (source is Shape shape)
        {
            var geometry = shape.RenderedGeometry;
            if (geometry != null && !geometry.IsEmpty())
            {
                // 需要考虑到 Shape 的 RenderTransform
                if (shape.RenderTransform != null && !shape.RenderTransform.Value.IsIdentity)
                {
                    geometry = geometry.Clone();
                    geometry.Transform = shape.RenderTransform;
                }
                return geometry;
            }
        }

        // 如果元素有 Geometry 类型的属性
        var geometryProperty = source.GetType().GetProperty("Geometry");
        if (geometryProperty != null && geometryProperty.GetValue(source) is Geometry geom)
        {
            return geom;
        }

        // 如果元素有 Data 属性（Path 控件）
        var dataProperty = source.GetType().GetProperty("Data");
        if (dataProperty != null && dataProperty.GetValue(source) is Geometry dataGeom)
        {
            return dataGeom;
        }

        // 如果元素有 Clip 属性
        var clipProperty = source.GetType().GetProperty("Clip");
        if (clipProperty != null && clipProperty.GetValue(source) is Geometry clipGeom)
        {
            return clipGeom;
        }

        // 默认为边界矩形
        return ExtractRenderBoundsGeometry(source);
    }


    private void SubscribeToElement(FrameworkElement element)
        {
            // 监听 SizeChanged 事件
            element.SizeChanged += OnSourceElementSizeChanged;

            // 监听 LayoutUpdated 事件
            element.LayoutUpdated += OnSourceElementLayoutUpdated;

            // 如果是 Shape，监听其 Geometry 相关属性变化
            if (element is Shape shape)
            {
                // 监听 Width/Height 变化
                var dpd = DependencyPropertyDescriptor.FromProperty(Shape.WidthProperty, typeof(Shape));
                if (dpd != null)
                    dpd.AddValueChanged(shape, OnSourceElementPropertyChanged);

                dpd = DependencyPropertyDescriptor.FromProperty(Shape.HeightProperty, typeof(Shape));
                if (dpd != null)
                    dpd.AddValueChanged(shape, OnSourceElementPropertyChanged);

                // 对于 Ellipse，还需要监听 Stretch 属性
                dpd = DependencyPropertyDescriptor.FromProperty(Shape.StretchProperty, typeof(Shape));
                if (dpd != null)
                    dpd.AddValueChanged(shape, OnSourceElementPropertyChanged);

                // 对于 Path 控件，监听 Data 属性
                if (shape is System.Windows.Shapes.Path path)
                {
                    dpd = DependencyPropertyDescriptor.FromProperty(System.Windows.Shapes.Path.DataProperty, typeof(System.Windows.Shapes.Path));
                    if (dpd != null)
                        dpd.AddValueChanged(path, OnSourceElementPropertyChanged);
                }

                // 对于 Rectangle，监听 RadiusX/RadiusY
                if (shape is Rectangle rect)
                {
                    dpd = DependencyPropertyDescriptor.FromProperty(Rectangle.RadiusXProperty, typeof(Rectangle));
                    if (dpd != null)
                        dpd.AddValueChanged(rect, OnSourceElementPropertyChanged);

                    dpd = DependencyPropertyDescriptor.FromProperty(Rectangle.RadiusYProperty, typeof(Rectangle));
                    if (dpd != null)
                        dpd.AddValueChanged(rect, OnSourceElementPropertyChanged);
                }
            }

            // 如果是 Border，监听 CornerRadius
            if (element is Border border)
            {
                var dpd = DependencyPropertyDescriptor.FromProperty(Border.CornerRadiusProperty, typeof(Border));
                if (dpd != null)
                    dpd.AddValueChanged(border, OnSourceElementPropertyChanged);
            }
        }

        private void UnsubscribeFromElement(FrameworkElement? element)
        {
            if (element == null) return;

            element.SizeChanged -= OnSourceElementSizeChanged;
            element.LayoutUpdated -= OnSourceElementLayoutUpdated;

            if (element is Shape shape)
            {
                var dpd = DependencyPropertyDescriptor.FromProperty(Shape.WidthProperty, typeof(Shape));
                if (dpd != null)
                    dpd.RemoveValueChanged(shape, OnSourceElementPropertyChanged);

                dpd = DependencyPropertyDescriptor.FromProperty(Shape.HeightProperty, typeof(Shape));
                if (dpd != null)
                    dpd.RemoveValueChanged(shape, OnSourceElementPropertyChanged);

                dpd = DependencyPropertyDescriptor.FromProperty(Shape.StretchProperty, typeof(Shape));
                if (dpd != null)
                    dpd.RemoveValueChanged(shape, OnSourceElementPropertyChanged);

                if (shape is System.Windows.Shapes.Path path)
                {
                    dpd = DependencyPropertyDescriptor.FromProperty(System.Windows.Shapes.Path.DataProperty, typeof(System.Windows.Shapes.Path));
                    if (dpd != null)
                        dpd.RemoveValueChanged(path, OnSourceElementPropertyChanged);
                }

                if (shape is Rectangle rect)
                {
                    dpd = DependencyPropertyDescriptor.FromProperty(Rectangle.RadiusXProperty, typeof(Rectangle));
                    if (dpd != null)
                        dpd.RemoveValueChanged(rect, OnSourceElementPropertyChanged);

                    dpd = DependencyPropertyDescriptor.FromProperty(Rectangle.RadiusYProperty, typeof(Rectangle));
                    if (dpd != null)
                        dpd.RemoveValueChanged(rect, OnSourceElementPropertyChanged);
                }
            }

            if (element is Border border)
            {
                var dpd = DependencyPropertyDescriptor.FromProperty(Border.CornerRadiusProperty, typeof(Border));
                if (dpd != null)
                    dpd.RemoveValueChanged(border, OnSourceElementPropertyChanged);
            }
        }

        private void OnSourceElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayoutFromSourceElement();
        }

        private void OnSourceElementLayoutUpdated(object? sender, EventArgs e)
        {
            // 布局更新时可能位置发生变化
            UpdateLayoutFromSourceElement();
        }

        private void OnSourceElementPropertyChanged(object? sender, EventArgs e)
        {
            UpdateLayoutFromSourceElement();
        }

        private Geometry ExtractActualShapeGeometry(FrameworkElement source)
        {
            var group = new GeometryGroup();

            double width = source.ActualWidth;
            double height = source.ActualHeight;

            if (width <= 0 || height <= 0)
            {
                // 如果还没有完成布局，使用 DesiredSize 或 Width/Height
                width = source.DesiredSize.Width > 0 ? source.DesiredSize.Width : source.Width;
                height = source.DesiredSize.Height > 0 ? source.DesiredSize.Height : source.Height;
                if (width <= 0) width = 100;
                if (height <= 0) height = 100;
            }

            // 根据元素类型创建对应的几何形状
            if (source is Ellipse)
            {
                group.Children.Add(new EllipseGeometry(new Rect(0, 0, width, height)));
            }
            else if (source is Rectangle rect)
            {
                double radiusX = rect.RadiusX;
                double radiusY = rect.RadiusY;
                if (radiusX > 0 || radiusY > 0)
                {
                    group.Children.Add(new RectangleGeometry(new Rect(0, 0, width, height), radiusX, radiusY));
                }
                else
                {
                    group.Children.Add(new RectangleGeometry(new Rect(0, 0, width, height)));
                }
            }
            else if (source is Border border)
            {
                var radius = border.CornerRadius;
                double topLeft = radius.TopLeft;
                group.Children.Add(new RectangleGeometry(new Rect(0, 0, width, height), topLeft, topLeft));
            }
            else if (source is System.Windows.Shapes.Path path && path.Data != null)
            {
                // 使用 Path 的 Data 几何，但需要调整边界
                var bounds = path.Data.Bounds;
                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    var transform = new TranslateTransform(-bounds.X, -bounds.Y);
                    var scaledGeometry = path.Data.Clone();
                    scaledGeometry.Transform = transform;

                    // 缩放到目标尺寸
                    var scaleTransform = new ScaleTransform(width / bounds.Width, height / bounds.Height);
                    scaledGeometry.Transform = scaleTransform;
                    group.Children.Add(scaledGeometry);
                }
                else
                {
                    group.Children.Add(path.Data);
                }
            }
            else
            {
                // 默认使用矩形
                group.Children.Add(new RectangleGeometry(new Rect(0, 0, width, height)));
            }

            return group;
        }

        private Geometry ExtractRenderBoundsGeometry(FrameworkElement source)
        {
            // 获取元素相对于自身的边界
            var bounds = new Rect(0, 0, source.ActualWidth, source.ActualHeight);

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                bounds = new Rect(0, 0, source.Width > 0 ? source.Width : 100,
                                        source.Height > 0 ? source.Height : 100);
            }

            return new RectangleGeometry(bounds);
        }

        private Geometry ExtractVisualTreeGeometry(FrameworkElement source)
        {
            var geometry = new GeometryGroup();

            void ExtractVisual(Visual visual, Transform? parentTransform = null)
            {
                // 获取视觉对象相对于源元素的边界
                var transform = visual.TransformToAncestor(source);
                if (transform != null)
                {
                    var bounds = VisualTreeHelper.GetDescendantBounds(visual);
                    if (!bounds.IsEmpty)
                    {
                        var transformedBounds = transform.TransformBounds(bounds);
                        geometry.Children.Add(new RectangleGeometry(transformedBounds));
                    }
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
                {
                    var child = VisualTreeHelper.GetChild(visual, i);
                    if (child is Visual childVisual)
                    {
                        ExtractVisual(childVisual);
                    }
                }
            }

            ExtractVisual(source);
            return geometry;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _panel = GetTemplateChild("PART_Panel") as PathListBoxPanel;
            UpdatePanelLayout();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PathListBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PathListBoxItem();
        }

        private void UpdatePanelLayout()
        {
            if (_panel == null) return;

            _panel.LayoutPath = LayoutPath;
            _panel.Distribution = Distribution;
            _panel.Orientation = Orientation;
            _panel.StartOffset = StartOffset;
            _panel.PathSpacing = PathSpacing;
            _panel.InvalidateArrange();
        }

        #endregion
    }

    /// <summary>
    /// Geometry 扩展方法
    /// </summary>
    public static class GeometryExtensions
    {
        public static bool IsEmpty(this Geometry geometry)
        {
            return geometry.Bounds.IsEmpty;
        }
    }

    public enum PathSourceType
    {
        Geometry,
        ActualShape,
        RenderBounds,
        VisualTree
    }

    public enum PathDistribution
    {
        Even,
        Proportional
    }

    public enum PathOrientation
    {
        None,
        OrientToPath
    }

    public class PathListBoxItem : ListBoxItem
    {
        static PathListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathListBoxItem),
                new FrameworkPropertyMetadata(typeof(PathListBoxItem)));
        }
    }



/// <summary>
/// 路径布局面板（支持动态路径变化）
/// </summary>
public class PathListBoxPanel : Panel
    {
        #region Dependency Properties

        public static readonly DependencyProperty LayoutPathProperty =
            DependencyProperty.Register(nameof(LayoutPath), typeof(Geometry),
                typeof(PathListBoxPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnLayoutPathChanged));

        public static readonly DependencyProperty DistributionProperty =
            DependencyProperty.Register(nameof(Distribution), typeof(PathDistribution),
                typeof(PathListBoxPanel), new FrameworkPropertyMetadata(PathDistribution.Even,
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(PathOrientation),
                typeof(PathListBoxPanel), new FrameworkPropertyMetadata(PathOrientation.None,
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty StartOffsetProperty =
            DependencyProperty.Register(nameof(StartOffset), typeof(double),
                typeof(PathListBoxPanel), new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty PathSpacingProperty =
            DependencyProperty.Register(nameof(PathSpacing), typeof(double),
                typeof(PathListBoxPanel), new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        #endregion

        #region Properties

        public Geometry LayoutPath
        {
            get => (Geometry)GetValue(LayoutPathProperty);
            set => SetValue(LayoutPathProperty, value);
        }

        public PathDistribution Distribution
        {
            get => (PathDistribution)GetValue(DistributionProperty);
            set => SetValue(DistributionProperty, value);
        }

        public PathOrientation Orientation
        {
            get => (PathOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double StartOffset
        {
            get => (double)GetValue(StartOffsetProperty);
            set => SetValue(StartOffsetProperty, value);
        }

        public double PathSpacing
        {
            get => (double)GetValue(PathSpacingProperty);
            set => SetValue(PathSpacingProperty, value);
        }

        #endregion

        #region Private Fields

        private Geometry? _cachedFlattenedPath;
        private List<Point> _cachedPathPoints = new List<Point>();
        private double _cachedPathLength;
        private bool _pathDirty = true;

        #endregion

        #region Event Handlers

        private static void OnLayoutPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as PathListBoxPanel;
            if (panel != null)
            {
                panel._pathDirty = true;
                panel.InvalidateArrange();
            }
        }

        #endregion

        #region Layout Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (LayoutPath == null || Children.Count == 0)
                return finalSize;

            // 更新缓存的路径数据
            UpdatePathCache();

            if (_cachedPathPoints.Count < 2)
                return finalSize;

            // 计算位置
            var positions = CalculatePositions();

            // 布局子元素
            for (int i = 0; i < Children.Count && i < positions.Count; i++)
            {
                ArrangeChild(Children[i], positions[i]);
            }

            return finalSize;
        }

        #endregion

        #region Path Caching

        private void UpdatePathCache()
        {
            if (!_pathDirty && _cachedFlattenedPath != null)
                return;

            // 展平路径以获得精确的点和切线
            _cachedFlattenedPath = LayoutPath.GetFlattenedPathGeometry();
            _cachedPathPoints.Clear();
            _cachedPathLength = 0;

            // 提取所有点
            if (_cachedFlattenedPath is PathGeometry pathGeometry)
            {
                Point? previousPoint = null;

                foreach (var figure in pathGeometry.Figures)
                {
                    AddPoint(figure.StartPoint);
                    previousPoint = figure.StartPoint;

                    foreach (var segment in figure.Segments)
                    {
                        if (segment is LineSegment line)
                        {
                            AddPoint(line.Point);
                            _cachedPathLength += Distance(previousPoint.Value, line.Point);
                            previousPoint = line.Point;
                        }
                        else if (segment is PolyLineSegment polyLine)
                        {
                            foreach (var point in polyLine.Points)
                            {
                                AddPoint(point);
                                _cachedPathLength += Distance(previousPoint.Value, point);
                                previousPoint = point;
                            }
                        }
                        else if (segment is ArcSegment arc)
                        {
                            // 对于弧线，采样更多点以获得更平滑的路径
                            var arcPoints = SampleArc(previousPoint.Value, arc);
                            foreach (var point in arcPoints)
                            {
                                AddPoint(point);
                                _cachedPathLength += Distance(previousPoint.Value, point);
                                previousPoint = point;
                            }
                        }
                        else if (segment is BezierSegment bezier)
                        {
                            var bezierPoints = SampleBezier(previousPoint.Value, bezier);
                            foreach (var point in bezierPoints)
                            {
                                AddPoint(point);
                                _cachedPathLength += Distance(previousPoint.Value, point);
                                previousPoint = point;
                            }
                        }
                        else if (segment is QuadraticBezierSegment quad)
                        {
                            var quadPoints = SampleQuadraticBezier(previousPoint.Value, quad);
                            foreach (var point in quadPoints)
                            {
                                AddPoint(point);
                                _cachedPathLength += Distance(previousPoint.Value, point);
                                previousPoint = point;
                            }
                        }
                    }
                }
            }

            _pathDirty = false;
        }

        private void AddPoint(Point point)
        {
            _cachedPathPoints.Add(point);
        }

        private List<Point> SampleArc(Point startPoint, ArcSegment arc)
        {
            var points = new List<Point>();
            int segments = 20; // 采样点数

            double angleStart = Math.Atan2(startPoint.Y, startPoint.X);
            double angleEnd = Math.Atan2(arc.Point.Y, arc.Point.X);

            if (arc.SweepDirection == SweepDirection.Clockwise)
            {
                if (angleEnd <= angleStart) angleEnd += 2 * Math.PI;
            }
            else
            {
                if (angleEnd >= angleStart) angleEnd -= 2 * Math.PI;
            }

            for (int i = 0; i <= segments; i++)
            {
                double t = (double)i / segments;
                double angle = angleStart + (angleEnd - angleStart) * t;
                double x = arc.Size.Width * Math.Cos(angle);
                double y = arc.Size.Height * Math.Sin(angle);
                points.Add(new Point(x, y));
            }

            return points;
        }

        private List<Point> SampleBezier(Point startPoint, BezierSegment bezier)
        {
            var points = new List<Point>();
            int segments = 20;

            for (int i = 0; i <= segments; i++)
            {
                double t = (double)i / segments;
                double u = 1 - t;

                double x = u * u * u * startPoint.X +
                          3 * u * u * t * bezier.Point1.X +
                          3 * u * t * t * bezier.Point2.X +
                          t * t * t * bezier.Point3.X;

                double y = u * u * u * startPoint.Y +
                          3 * u * u * t * bezier.Point1.Y +
                          3 * u * t * t * bezier.Point2.Y +
                          t * t * t * bezier.Point3.Y;

                points.Add(new Point(x, y));
            }

            return points;
        }

        private List<Point> SampleQuadraticBezier(Point startPoint, QuadraticBezierSegment quad)
        {
            var points = new List<Point>();
            int segments = 20;

            for (int i = 0; i <= segments; i++)
            {
                double t = (double)i / segments;
                double u = 1 - t;

                double x = u * u * startPoint.X +
                          2 * u * t * quad.Point1.X +
                          t * t * quad.Point2.X;

                double y = u * u * startPoint.Y +
                          2 * u * t * quad.Point1.Y +
                          t * t * quad.Point2.Y;

                points.Add(new Point(x, y));
            }

            return points;
        }

        #endregion

        #region Position Calculation

        private List<PositionInfo> CalculatePositions()
        {
            var positions = new List<PositionInfo>();

            if (_cachedPathPoints.Count == 0 || _cachedPathLength <= 0)
                return positions;

            if (Distribution == PathDistribution.Even)
            {
                // 均匀分布
                int count = Children.Count;
                for (int i = 0; i < count; i++)
                {
                    double normalizedPos = (StartOffset + (double)i / count) % 1.0;
                    double distance = normalizedPos * _cachedPathLength;
                    var pointInfo = GetPointAtDistance(distance);

                    if (pointInfo.Point.HasValue)
                    {
                        positions.Add(new PositionInfo
                        {
                            Point = pointInfo.Point.Value,
                            Angle = pointInfo.Angle
                        });
                    }
                }
            }
            else
            {
                // 按比例分布
                double totalWidth = Children.Cast<UIElement>().Sum(c => c.DesiredSize.Width);
                double accumulated = StartOffset;

                for (int i = 0; i < Children.Count; i++)
                {
                    var child = Children[i];
                    double proportion = child.DesiredSize.Width / totalWidth;
                    accumulated += proportion;
                    if (accumulated > 1) accumulated -= 1;

                    double distance = accumulated * _cachedPathLength;
                    var pointInfo = GetPointAtDistance(distance);

                    if (pointInfo.Point.HasValue)
                    {
                        positions.Add(new PositionInfo
                        {
                            Point = pointInfo.Point.Value,
                            Angle = pointInfo.Angle
                        });
                    }
                }
            }

            // 应用间距
            ApplySpacing(positions);

            return positions;
        }

        private (Point? Point, double? Angle) GetPointAtDistance(double distance)
        {
            if (_cachedPathPoints.Count < 2)
                return (null, null);

            double accumulated = 0;

            for (int i = 1; i < _cachedPathPoints.Count; i++)
            {
                double segmentLength = Distance(_cachedPathPoints[i - 1], _cachedPathPoints[i]);

                if (accumulated + segmentLength >= distance)
                {
                    double t = (distance - accumulated) / segmentLength;
                    var point = new Point(
                        _cachedPathPoints[i - 1].X + (_cachedPathPoints[i].X - _cachedPathPoints[i - 1].X) * t,
                        _cachedPathPoints[i - 1].Y + (_cachedPathPoints[i].Y - _cachedPathPoints[i - 1].Y) * t
                    );

                    // 计算切线角度
                    double? angle = null;
                    if (Orientation == PathOrientation.OrientToPath)
                    {
                        angle = Math.Atan2(_cachedPathPoints[i].Y - _cachedPathPoints[i - 1].Y,
                                          _cachedPathPoints[i].X - _cachedPathPoints[i - 1].X) * 180 / Math.PI;
                    }

                    return (point, angle);
                }

                accumulated += segmentLength;
            }

            return (_cachedPathPoints.Last(), null);
        }

        private void ApplySpacing(List<PositionInfo> positions)
        {
            if (PathSpacing <= 0 || positions.Count < 2)
                return;

            // 简单实现：调整位置以保持间距
            // 可根据需要实现更复杂的间距逻辑
        }

        private void ArrangeChild(UIElement child, PositionInfo position)
        {
            var childSize = child.DesiredSize;
            var point = position.Point;

            var rect = new Rect(point.X - childSize.Width / 2,
                               point.Y - childSize.Height / 2,
                               childSize.Width,
                               childSize.Height);

            if (Orientation == PathOrientation.OrientToPath && position.Angle.HasValue)
            {
                var transform = new TransformGroup();
                transform.Children.Add(new RotateTransform(position.Angle.Value + 90,
                    childSize.Width / 2, childSize.Height / 2));
                child.RenderTransform = transform;
            }
            else
            {
                child.RenderTransform = null;
            }

            child.Arrange(rect);
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        #endregion

        #region Helper Classes

        private class PositionInfo
        {
            public Point Point { get; set; }
            public double? Angle { get; set; }
        }

        #endregion
    }

