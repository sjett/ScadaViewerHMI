// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HMI.Editor.Surface;

/// <summary>
/// 设计器画布
/// </summary>
public class DesignerCanvas : Canvas, IDesignSurface
{
    /// <summary>
    /// 页面
    /// </summary>
    public static readonly DependencyProperty PageProperty =
        DependencyProperty.Register("Page", typeof(IProjectPageViewModel), typeof(DesignerCanvas), null);
    /// <summary>
    /// 缩放因子
    /// </summary>
    public static readonly DependencyProperty ZoomFactorProperty =
        DependencyProperty.Register("ZoomFactor", typeof(double), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnZoomFactorChanged)));
    /// <summary>
    /// 反向缩放因子
    /// </summary>
    public static readonly DependencyProperty InverseZoomFactorProperty =
        DependencyProperty.Register("InverseZoomFactor", typeof(double), typeof(DesignerCanvas), new PropertyMetadata(1.0));
    /// <summary>
    /// 标记位置
    /// </summary>
    public static readonly DependencyProperty MarksProperty =
        DependencyProperty.Register("Marks", typeof(MarksLocation), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(MarksLocation.Up, FrameworkPropertyMetadataOptions.AffectsRender));
    /// <summary>
    /// 网格线画笔
    /// </summary>
    public static readonly DependencyProperty GridPenProperty =
        DependencyProperty.Register("GridPen", typeof(Pen), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(new Pen(Brushes.LightGray, 0.5)));
    /// <summary>
    /// 显示网格线
    /// </summary>
    public static readonly DependencyProperty ShowGridLinesProperty =
        DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
    /// <summary>
    /// 吸附到网格
    /// </summary>
    public static readonly DependencyProperty SnapToGridProperty =
        DependencyProperty.Register("SnapToGrid", typeof(bool), typeof(DesignerCanvas), new PropertyMetadata(true));
    /// <summary>
    /// 显示标尺辅助线
    /// </summary>
    public static readonly DependencyProperty ShowRulerGuidesProperty =
        DependencyProperty.Register("ShowRulerGuides", typeof(bool), typeof(DesignerCanvas),
            new PropertyMetadata(true, new PropertyChangedCallback(OnShowRulerGuidesChanged)));
    /// <summary>
    /// 吸附到标尺辅助线
    /// </summary>
    public static readonly DependencyProperty SnapToRulerGuidesProperty =
        DependencyProperty.Register("SnapToRulerGuides", typeof(bool), typeof(DesignerCanvas), new PropertyMetadata(true));
    /// <summary>
    /// 网格大小
    /// </summary>
    public static readonly DependencyProperty GridSizeProperty =
        DependencyProperty.Register("GridSize", typeof(double), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.AffectsRender));
    /// <summary>
    /// 网格吸附阈值
    /// </summary>
    public static readonly DependencyProperty GridSnapThresholdProperty =
        DependencyProperty.Register("GridSnapThreshold", typeof(double), typeof(DesignerCanvas),
            new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));
    /// <summary>
    /// 标尺大小
    /// </summary>
    public static readonly DependencyProperty RulerSizeProperty =
        DependencyProperty.Register("RulerSize", typeof(double), typeof(DesignerCanvas), new PropertyMetadata(50.0));

    private bool renderedOnce;
    private static string ClipboardFormat = DataFormats.Xaml;
    /// <summary>
    /// 缩放因子改变事件
    /// </summary>
    public event EventHandler<ZoomFactorChangedEventArgs> ZoomFactorChanged;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DesignerCanvas()
    {
        // 注册打印命令
        base.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, new ExecutedRoutedEventHandler(this.Print_Executed)));
        base.AllowDrop = true;//允许拖放
        this.CurrentAction = new SelectAction(this);//框选操作
    }

    /// <summary>
    /// 页面
    /// </summary>
    public IProjectPageViewModel Page
    {
        get
        {
            return (IProjectPageViewModel)base.GetValue(PageProperty);
        }
        set
        {
            base.SetValue(PageProperty, value);
        }
    }
    /// <summary>
    /// 当前操作
    /// </summary>
    public DesignerAction CurrentAction
    {
        get;
        set;
    }
    /// <summary>
    /// 缩放因子
    /// </summary>
    public double ZoomFactor
    {
        get
        {
            return (double)base.GetValue(ZoomFactorProperty);
        }
        set
        {
            base.SetValue(ZoomFactorProperty, value);
        }
    }
    /// <summary>
    /// 反向缩放因子
    /// </summary>
    public double InverseZoomFactor
    {
        get
        {
            return (double)base.GetValue(InverseZoomFactorProperty);
        }
        set
        {
            base.SetValue(InverseZoomFactorProperty, value);
        }
    }
    /// <summary>
    /// 标记位置
    /// </summary>
    public MarksLocation Marks
    {
        get
        {
            return (MarksLocation)base.GetValue(MarksProperty);
        }
        set
        {
            base.SetValue(MarksProperty, value);
        }
    }
    /// <summary>
    /// 网格线画笔
    /// </summary>
    public Pen GridPen
    {
        get
        {
            return (Pen)base.GetValue(GridPenProperty);
        }
        set
        {
            base.SetValue(GridPenProperty, value);
        }
    }
    /// <summary>
    /// 显示网格线
    /// </summary>
    public bool ShowGridLines
    {
        get
        {
            return (bool)base.GetValue(ShowGridLinesProperty);
        }
        set
        {
            base.SetValue(ShowGridLinesProperty, value);
        }
    }
    /// <summary>
    /// 吸附到网格
    /// </summary>
    public bool SnapToGrid
    {
        get
        {
            return (bool)base.GetValue(SnapToGridProperty);
        }
        set
        {
            base.SetValue(SnapToGridProperty, value);
        }
    }
    /// <summary>
    /// 显示标尺辅助线
    /// </summary>
    public bool ShowRulerGuides
    {
        get
        {
            return (bool)base.GetValue(ShowRulerGuidesProperty);
        }
        set
        {
            base.SetValue(ShowRulerGuidesProperty, value);
        }
    }
    /// <summary>
    /// 吸附到标尺辅助线
    /// </summary>
    public bool SnapToRulerGuides
    {
        get
        {
            return (bool)base.GetValue(SnapToRulerGuidesProperty);
        }
        set
        {
            base.SetValue(SnapToRulerGuidesProperty, value);
        }
    }
    /// <summary>
    /// 网格大小
    /// </summary>
    public double GridSize
    {
        get
        {
            return (double)base.GetValue(GridSizeProperty);
        }
        set
        {
            base.SetValue(GridSizeProperty, value);
        }
    }
    /// <summary>
    /// 网格吸附阈值
    /// </summary>
    public double GridSnapThreshold
    {
        get
        {
            return (double)base.GetValue(GridSnapThresholdProperty);
        }
        set
        {
            base.SetValue(GridSnapThresholdProperty, value);
        }
    }
    /// <summary>
    /// 标尺大小
    /// </summary>
    public double RulerSize
    {
        get
        {
            return (double)base.GetValue(RulerSizeProperty);
        }
        set
        {
            base.SetValue(RulerSizeProperty, value);
        }
    }
    /// <summary>
    /// 鼠标按下事件
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Source == this)
        {
            this.CurrentAction.OnMouseDown(e);
        }
    }
    /// <summary>
    /// 鼠标移动事件
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        this.CurrentAction.OnMouseMove(e);
    }
    /// <summary>
    /// 渲染事件
    /// </summary>
    /// <param name="dc"></param>
    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        this.renderedOnce = true;//记录已渲染过一次
        Pen pen = new Pen(Brushes.Transparent, 0.0);
        Rect rectangle = new Rect(0.0, 0.0, base.ActualWidth, base.ActualHeight);
        dc.DrawRectangle(base.Background, pen, rectangle);//绘制背景
        if (this.ShowGridLines)//显示网格线
        {
            this.RenderGrid(dc);//绘制网格线
        }
    }
    /// <summary>
    /// 获取画布快照
    /// </summary>
    /// <returns></returns>
    public BitmapSource GetSnapshot()
    {
        if (!base.IsVisible && !this.renderedOnce)
        {
            base.Arrange(new Rect(this.GridSize, this.GridSize, base.Width + this.GridSize, base.Height + this.GridSize));
        }
        return VisualUtility.CreateBitmapFromVisual(base.Width, base.Height, this, true);
    }
    /// <summary>
    /// 缩放因子改变事件
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        double num = (double)e.NewValue;
        DesignerCanvas designerCanvas = d as DesignerCanvas;
        designerCanvas.InverseZoomFactor = 1.0 / num;
        designerCanvas.FireZoomFactorChanged(num);
    }
    /// <summary>
    /// 触发缩放因子改变事件
    /// </summary>
    /// <param name="newValue"></param>
    private void FireZoomFactorChanged(double newValue)
    {
        ZoomFactorChangedEventArgs e = new ZoomFactorChangedEventArgs
        {
            zoomFactor = newValue
        };
        this.ZoomFactorChanged?.Invoke(this, e);
    }
    /// <summary>
    /// 显示标尺辅助线改变事件
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnShowRulerGuidesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        DesignerCanvas designerCanvas = d as DesignerCanvas;
        Visibility rulerGuidesVisibility = ((bool)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        designerCanvas.SetRulerGuidesVisibility(rulerGuidesVisibility);
    }
    /// <summary>
    /// 设置标尺辅助线可见性
    /// </summary>
    /// <param name="visibility"></param>
    private void SetRulerGuidesVisibility(Visibility visibility)
    {
        IEnumerable<RulerGuide> rulerGuides = this.GetRulerGuides();
        foreach (RulerGuide current in rulerGuides)
        {
            current.Visibility = visibility;
        }
    }
    /// <summary>
    /// 渲染网格线
    /// </summary>
    /// <param name="dc"></param>
    private void RenderGrid(DrawingContext dc)
    {
        this.GridPen = new Pen(this.GridPen.Brush, 1.0 / this.ZoomFactor)
        {
            StartLineCap = this.GridPen.StartLineCap,
            EndLineCap = this.GridPen.EndLineCap,
            DashCap = this.GridPen.DashCap,
            LineJoin = this.GridPen.LineJoin,
            MiterLimit = this.GridPen.MiterLimit,
            DashStyle = this.GridPen.DashStyle
        };
        this.DrawGridVerticalLines(dc);
        this.DrawGridHorizontalLines(dc);
    }
    /// <summary>
    /// 绘制水平网格线
    /// </summary>
    /// <param name="dc"></param>
    private void DrawGridHorizontalLines(DrawingContext dc)
    {
        for (double num = 0.0; num < base.Height; num += this.GridSize)
        {
            dc.DrawLine(this.GridPen, new Point(0.0, num), new Point(base.Width, num));
        }
    }
    /// <summary>
    /// 绘制垂直网格线
    /// </summary>
    /// <param name="dc"></param>
    private void DrawGridVerticalLines(DrawingContext dc)
    {
        for (double num = 0.0; num < base.Width; num += this.GridSize)
        {
            dc.DrawLine(this.GridPen, new Point(num, 0.0), new Point(num, base.Height));
        }
    }
    /// <summary>
    /// 获取最接近的网格吸附点
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private Point GetClosestGridSnapPoint(Point point)
    {
        return new Point
        {
            X = System.Math.Round(point.X / this.GridSize) * this.GridSize,
            Y = System.Math.Round(point.Y / this.GridSize) * this.GridSize
        };
    }
    /// <summary>
    /// 创建标尺辅助线
    /// </summary>
    /// <param name="orientation"></param>
    /// <param name="coords"></param>
    public void CreateRulerGuide(Orientation orientation, Point coords)
    {
        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (adornerLayer != null)
        {
            RulerGuide rulerGuide = new RulerGuide(this, orientation, coords);
            adornerLayer.Add(rulerGuide);
            rulerGuide.CaptureMouse();
        }
    }
    /// <summary>
    /// 移除标尺辅助线
    /// </summary>
    /// <param name="guide"></param>
    public void RemoveRulerGuide(RulerGuide guide)
    {
        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (adornerLayer != null)
        {
            adornerLayer.Remove(guide);
        }
    }
    /// <summary>
    /// 获取最接近的吸附点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="snapMode"></param>
    /// <returns></returns>
    public Point GetClosestSnapPoint(Point point, SnapMode snapMode)
    {
        List<Point> list = [new Point(double.MaxValue, double.MaxValue)];
        if (snapMode == SnapMode.Regular)
        {
            if (this.SnapToGrid)
            {
                list.Add(this.GetClosestGridSnapPoint(point));
            }
            if (this.SnapToRulerGuides)
            {
                list.AddRange(this.GetRulerGuideSnapPoints(point));
            }
        }
        double x2 = (
            from s in list
            select s.X into x
            orderby System.Math.Abs(x - point.X)
            select x).First<double>();
        double y2 = (
            from s in list
            select s.Y into y
            orderby System.Math.Abs(y - point.Y)
            select y).First<double>();
        return new Point(x2, y2);
    }
    /// <summary>
    /// 获取标尺辅助线吸附点
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private IEnumerable<Point> GetRulerGuideSnapPoints(Point point)
    {
        IEnumerable<RulerGuide> rulerGuides = this.GetRulerGuides();
        return
            from guide in rulerGuides
            select guide.GetSnapPoint(point);
    }
    /// <summary>
    /// 获取标尺辅助线集合
    /// </summary>
    /// <returns></returns>
    private IEnumerable<RulerGuide> GetRulerGuides()
    {
        IEnumerable<RulerGuide> result = new RulerGuide[0];
        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (adornerLayer != null)
        {
            Adorner[] adorners = adornerLayer.GetAdorners(this);
            if (adorners != null)
            {
                result = adorners.OfType<RulerGuide>();
            }
        }
        return result;
    }

    /// <summary>
    /// 打印命令执行事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        PrintDialog printDialog = new PrintDialog();
        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(this, "WPF 画布");//打印当前画布
        }
    }

}
