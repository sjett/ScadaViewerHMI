// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 标尺辅助线
    /// </summary>
    public class RulerGuide : Adorner
    {
        private Point coords;
        private readonly Orientation orientation;
        private readonly DesignerCanvas designer;
        private readonly Pen guidePen = new Pen(Brushes.DarkBlue, 1.0);
        private readonly Pen movingPen = new Pen(Brushes.Red, 1.0);
        private Pen activePen;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="designer">设计器画布</param>
        /// <param name="orientation">方向</param>
        /// <param name="coords">位置坐标</param>
        public RulerGuide(DesignerCanvas designer, Orientation orientation, Point coords) : base(designer)
        {
            this.designer = designer;
            this.orientation = orientation;
            this.coords = coords;
            this.guidePen.DashStyle = DashStyles.Dot;
            this.activePen = this.guidePen;
            if (this.orientation == Orientation.Horizontal)
            {
                base.Cursor = Cursors.SizeNS;
                return;
            }
            base.Cursor = Cursors.SizeWE;
        }
        /// <summary>
        /// 渲染辅助线
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            Size desiredSize = base.AdornedElement.DesiredSize;
            Rect rulerRect = this.GetRulerRect(ref desiredSize, 0, this.designer.InverseZoomFactor);
            drawingContext.DrawLine(new Pen(this.activePen.Brush, this.designer.InverseZoomFactor)
            {
                StartLineCap = this.activePen.StartLineCap,
                EndLineCap = this.activePen.EndLineCap,
                DashCap = this.activePen.DashCap,
                LineJoin = this.activePen.LineJoin,
                MiterLimit = this.activePen.MiterLimit,
                DashStyle = this.activePen.DashStyle
            }, rulerRect.TopLeft, rulerRect.BottomRight);
        }
        /// <summary>
        /// 获取标尺辅助线的矩形区域
        /// </summary>
        /// <param name="size"></param>
        /// <param name="rulerEdgeSize"></param>
        /// <param name="inverseZoomFactor"></param>
        /// <returns></returns>
        private Rect GetRulerRect(ref Size size, int rulerEdgeSize, double inverseZoomFactor)
        {
            Rect result = default(Rect);
            if (this.orientation == Orientation.Horizontal)
            {
                result.X = (double)(-(double)rulerEdgeSize);
                result.Y = this.coords.Y;
                result.Width = size.Width * inverseZoomFactor + (double)rulerEdgeSize;
                result.Height = 0.0;
            }
            else
            {
                result.X = this.coords.X;
                result.Y = (double)(-(double)rulerEdgeSize);
                result.Width = 0.0;
                result.Height = size.Height * inverseZoomFactor + (double)rulerEdgeSize;
            }
            return result;
        }
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.coords = e.GetPosition(this.designer);
                base.InvalidateVisual();
            }
        }
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.CaptureMouse();
            if (e.ClickCount > 1)
            {
                base.ReleaseMouseCapture();
                this.designer.RemoveRulerGuide(this);
            }
        }
        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this.designer);
            base.ReleaseMouseCapture();
            if ((this.orientation == Orientation.Vertical && (position.X < 0.0 || position.X > this.designer.ActualWidth)) || (this.orientation == Orientation.Horizontal && position.Y < 0.0) || position.Y > this.designer.ActualHeight)
            {
                this.designer.RemoveRulerGuide(this);
            }
        }
        /// <summary>
        /// 获得鼠标捕获事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            base.OnGotMouseCapture(e);
            this.activePen = this.movingPen;
            base.InvalidateVisual();
        }
        /// <summary>
        /// 失去鼠标捕获事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            this.activePen = this.guidePen;
            base.InvalidateVisual();
        }
        /// <summary>
        /// 获取捕捉点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point GetSnapPoint(Point point)
        {
            if (this.orientation == Orientation.Horizontal)
            {
                return new Point(double.MaxValue, this.coords.Y);
            }
            return new Point(this.coords.X, double.MaxValue);
        }
    }
}
