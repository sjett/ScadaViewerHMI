// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.Views
{
    /// <summary>
    /// 内容控件容器
    /// </summary>
    //[TemplatePart(Name ="uxContainerCanvas", Type = typeof(Canvas))] //声明控件模板中必须包含的命名部件
    public class ControlContainer : ContentControl, IControlContainer, INotifyPropertyChanged
    {
        private IActionMonitor _iactionMonitor;
        private bool _isDragEnabled = true;
        private bool _isRotateEnabled = true;
        private bool _isResizeEnabled;
        private Point _startedPoint;
        private bool _selectArea;
        private bool _taped;
        private List<ControlContainer> hitedControlContainers;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// 角度改变事件
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<double>>? AngleChanged;
        /// <summary>
        /// 角度依赖属性
        /// </summary>
        public static readonly DependencyProperty AngleProperty =
                        DependencyProperty.Register(
                            nameof(Angle),
                            typeof(double),
                            typeof(ControlContainer),
                            new PropertyMetadata(0.0, (d, e) => {
                                // 如果是ControlContainer类型，则触发角度改变事件
                                if (d is ControlContainer controlContainer)
                                {
                                    // 触发角度改变事件
                                    controlContainer.AngleChanged?.Invoke(d,
                                        new ValueChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue));
                                }
                            }));

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ControlContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlContainer), new FrameworkPropertyMetadata(typeof(ControlContainer)));
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public ICommonControlViewModel? ViewModel
        {
            get
            {
                return DataContext as ICommonControlViewModel;
            }
        }
        /// <summary>
        /// 容器画布
        /// </summary>
        public Canvas? UxContainerCanvas { get; set; }
        /// <summary>
        /// 是否为组合控件
        /// </summary>
        public bool IsGroup => this.ViewModel is ISelectionControlViewModel;

        /// <summary>
        /// 是否启用拖动
        /// </summary>
        public bool IsDragEnabled
        {
            get
            {
                return this._isDragEnabled;
            }
            private set
            {
                this._isDragEnabled = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 是否启用旋转
        /// </summary>
        public bool IsRotateEnabled
        {
            get
            {
                return this._isRotateEnabled;
            }
            private set
            {
                this._isRotateEnabled = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get => (double)GetValue(AngleProperty); set => SetValue(AngleProperty, value); }

        /// <summary>
        /// 是否启用调整大小
        /// </summary>
        public bool IsResizeEnabled
        {
            get
            {
                return this._isResizeEnabled;
            }
            set
            {
                if (this._isResizeEnabled != value)
                {
                    this._isResizeEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 获取布局裁剪区域
        /// </summary>
        /// <param name="layoutSlotSize"></param>
        /// <returns></returns>
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            if (!ClipToBounds)
            {
                return null;
            }
            return base.GetLayoutClip(layoutSlotSize);
        }
        /// <summary>
        /// 获取内边界矩形
        /// </summary>
        /// <returns></returns>
        public Rect GetInnerBoundingRect()
        {
            Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.UxContainerCanvas!.Children.OfType<ContentControl>().First<ContentControl>());
            return this.UxContainerCanvas.TransformToAncestor(this).TransformBounds(descendantBounds);
        }
        /// <summary>
        /// 获取外边界矩形
        /// </summary>
        /// <returns></returns>
        public Rect GetOuterBoundingRect()
        {
            Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this.UxContainerCanvas);
            return this.UxContainerCanvas!.TransformToAncestor(this).TransformBounds(descendantBounds);
        }
        /// <summary>
        /// 转换控件点到指定元素
        /// </summary>
        /// <param name="point"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        public Point TranslateControlPoint(Point point, UIElement relativeTo)
        {
            return this.UxContainerCanvas!.TranslatePoint(point, relativeTo);
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="mode"></param>
        public void Rotate(double angle, TransformMode mode)
        {
            if (mode == TransformMode.Relative)
            {
                this.Angle += angle;
                return;
            }
            this.Angle = angle;
        }
        /// <summary>
        /// 应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UxContainerCanvas = (base.GetTemplateChild("uxContainerCanvas") as Canvas);
        }
        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="coords">开始坐标</param>
        public void DragStarted(Point coords)
        {
            this._iactionMonitor = this.ViewModel!.ActionMonitor;
            this._iactionMonitor.StartGrouping();
            this._startedPoint = coords;
            this._taped = true;
            this._selectArea = false;
        }
        /// <summary>
        /// 拖动变化
        /// </summary>
        public void DragDelta()
        {
            if (this._taped && !this.IsGroup && !this.ViewModel!.IsSelected)//单击选择非组合控件
            {
                if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    this.ViewModel!.Select();//选中
                }
                else
                {
                    this.ViewModel!.ToggleSelection();//切换选择
                }
                Keyboard.Focus(this);
            }
            this._taped = false;
            this._selectArea = true;
        }
        /// <summary>
        /// 拖动完成
        /// </summary>
        public void DragCompleted()
        {
            if (this._taped)//单击选择控件
            {
                if (!this.IsGroup)//非组合控件
                {
                    if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        this.ViewModel!.Select();//选中
                    }
                    else
                    {
                        this.ViewModel!.ToggleSelection();//切换选择
                    }
                    Keyboard.Focus(this);
                }
                else
                {//组合控件，命中测试子控件，选中子控件
                    Canvas canvas = getParentCanvas();
                    Point point = transformToAncestorPoint(canvas);
                    hitControlContainer(canvas, point);
                    triggerClicked(point, canvas);
                }
            }
            this._taped = false;
            if (this._selectArea)//拖动区域选择控件
            {
                this._iactionMonitor.EndGrouping();
                return;
            }
            this._iactionMonitor.CancelAction();
        }
        /// <summary>
        /// 获取父级画布
        /// </summary>
        /// <returns></returns>
        private Canvas getParentCanvas()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (parent != null && !(parent is Canvas))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as Canvas;
        }
        /// <summary>
        /// 转换到祖先点
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private Point transformToAncestorPoint(Canvas canvas)
        {
            GeneralTransform generalTransform = this.TransformToAncestor(canvas);
            return generalTransform.Transform(this._startedPoint);
        }
        /// <summary>
        /// 命中测试控件容器
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        private void hitControlContainer(Canvas canvas, Point point)
        {
            this.hitedControlContainers = new List<ControlContainer>();
            VisualTreeHelper.HitTest(canvas, 
                new HitTestFilterCallback(this.potentialHitTestTarget), 
                new HitTestResultCallback(this.hitTestResultCallback), 
                new PointHitTestParameters(point));
        }
        /// <summary>
        /// 触发点击事件
        /// </summary>
        /// <param name="point"></param>
        /// <param name="canvas"></param>
        private void triggerClicked(Point point, Canvas canvas)
        {
            for (int i = 0; i < hitedControlContainers.Count; i++)
            {
                ControlContainer controlContainer = hitedControlContainers[i];
                Rect outerBoundingRect = controlContainer.GetOuterBoundingRect();
                if (GeometryService.IsPointInPolygon(point, new Point[]
                {
                    controlContainer.TranslatePoint(outerBoundingRect.TopLeft, canvas),
                    controlContainer.TranslatePoint(outerBoundingRect.TopRight, canvas),
                    controlContainer.TranslatePoint(outerBoundingRect.BottomRight, canvas),
                    controlContainer.TranslatePoint(outerBoundingRect.BottomLeft, canvas)
                }))
                {
                    if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        controlContainer.ViewModel!.Select();
                    }
                    else
                    {
                        controlContainer.ViewModel!.ToggleSelection();
                    }
                    Keyboard.Focus(controlContainer);
                    return;
                }
            }
        }
        /// <summary>
        /// 命中测试结果回调
        /// </summary>
        /// <param name="htr"></param>
        /// <returns></returns>
        private HitTestResultBehavior hitTestResultCallback(HitTestResult htr)
        {
            return HitTestResultBehavior.Continue;
        }
        /// <summary>
        /// 潜在命中测试目标
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private HitTestFilterBehavior potentialHitTestTarget(DependencyObject d)
        {
            if (!(d is ControlContainer))
            {
                return HitTestFilterBehavior.ContinueSkipSelf;
            }
            if (d != this)
            {
                ControlContainer item = d as ControlContainer;
                this.hitedControlContainers.Add(item);
            }
            return HitTestFilterBehavior.Continue;
        }

        #region IControlContainer 接口实现

        /// <summary>
        /// 更新布局
        /// </summary>
        void IControlContainer.UpdateLayout()
        {
            base.UpdateLayout();
        }
        /// <summary>
        /// 转换到祖先元素
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        GeneralTransform IControlContainer.TransformToAncestor(Visual visual)
        {
            return base.TransformToAncestor(visual);
        }
        /// <summary>
        /// 转换到后代元素
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        GeneralTransform IControlContainer.TransformToDescendant(Visual visual)
        {
            return base.TransformToDescendant(visual);
        }
        /// <summary>
        /// 转换到指定元素
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        GeneralTransform IControlContainer.TransformToVisual(Visual visual)
        {
            return base.TransformToVisual(visual);
        }

        #endregion
    }
}
