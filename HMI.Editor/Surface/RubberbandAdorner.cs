// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Editor.Surface
{
    /// <summary>
    /// 装饰器：选择框
    /// </summary>
    public class RubberbandAdorner : Adorner
    {
        private const int SelectionCornerRadius = 2;
        private const double SelectionStrokeThickness = 0.5;
        private readonly IProjectPageViewModel pageViewModel;
        private readonly Point? startPoint;
        private Point? endPoint;
        private readonly Canvas designerCanvas;
        private readonly LinearGradientBrush selectionFill;
        private readonly Pen selectionInnerStroke;
        private readonly Pen selectionOuterStroke;
        private readonly IList<ICommonControlViewModel> selection = new List<ICommonControlViewModel>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="designerCanvas"></param>
        /// <param name="dragStartPoint"></param>
        /// <param name="pageViewModel"></param>
        public RubberbandAdorner(Canvas designerCanvas, Point? dragStartPoint, IProjectPageViewModel pageViewModel) : base(designerCanvas)
        {
            base.SnapsToDevicePixels = true;// 启用像素对齐
            this.pageViewModel = pageViewModel;// 页面视图模型
            this.designerCanvas = designerCanvas;// 设计器画布
            this.startPoint = dragStartPoint;// 起始点
            this.selectionFill = new LinearGradientBrush(Color.FromArgb(128, 220, 235, 252), Color.FromArgb(128, 193, 219, 252), 90.0);
            this.selectionInnerStroke = new Pen(new SolidColorBrush(Color.FromArgb(255, 125, 162, 206)), SelectionStrokeThickness);
            this.selectionOuterStroke = new Pen(new SolidColorBrush(Color.FromArgb(255, 235, 244, 253)), SelectionStrokeThickness);
        }
        /// <summary>
        /// 鼠标移动事件处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!base.IsMouseCaptured)
                {
                    base.CaptureMouse();
                }
                this.endPoint = new Point?(e.GetPosition(this));
                this.UpdateSelection();
                base.InvalidateVisual();
            }
            else
            {
                if (base.IsMouseCaptured)
                {
                    base.ReleaseMouseCapture();
                }
            }
            e.Handled = true;
        }
        /// <summary>
        ///	清除选择
        /// </summary>
        /// <param name="isShiftPressed"></param>
        /// <param name="isControlPressed"></param>
        private void ClearSelection(bool isShiftPressed, bool isControlPressed)
        {
            if (!isShiftPressed && !isControlPressed)
            {
                this.pageViewModel.SetSelection(null);
            }
        }
        /// <summary>
        /// 移除装饰器
        /// </summary>
        private void RemoveAdorner()
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.designerCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }
        /// <summary>
        /// 提交选择更改
        /// </summary>
        private void CommitSelectionChanges()
        {
            bool isShiftPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
            bool flag = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
            this.ClearSelection(isShiftPressed, flag);
            foreach (ICommonControlViewModel current in this.selection)
            {
                this.pageViewModel.UpdateSelection(current, flag);
            }
        }
        /// <summary>
        /// 鼠标释放事件处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (base.IsMouseCaptured)
            {
                base.ReleaseMouseCapture();
            }
            this.RemoveAdorner();
            this.CommitSelectionChanges();
            e.Handled = true;
        }
        /// <summary>
        /// 绘制内部选择矩形
        /// </summary>
        /// <param name="dc"></param>
        private void DrawInnerSelectionRectangle(DrawingContext dc)
        {
            Rect rectangle = new Rect(this.startPoint.Value, this.endPoint.Value);
            Pen pen = this.selectionInnerStroke.Clone();
            pen.Thickness *= this.pageViewModel.InverseScale;
            dc.DrawRoundedRectangle(this.selectionFill, pen, rectangle, SelectionCornerRadius * this.pageViewModel.InverseScale, SelectionCornerRadius * this.pageViewModel.InverseScale);
        }
        /// <summary>
        /// 绘制外部选择矩形
        /// </summary>
        /// <param name="dc"></param>
        private void DrawOuterSelectionRectangle(DrawingContext dc)
        {
            Rect rectangle = new Rect(this.startPoint.Value, this.endPoint.Value);
            rectangle.Offset(1.0 * this.pageViewModel.InverseScale, 1.0 * this.pageViewModel.InverseScale);
            rectangle.Inflate(-3.0 * this.pageViewModel.InverseScale, -3.0 * this.pageViewModel.InverseScale);
            Pen pen = this.selectionOuterStroke.Clone();
            pen.Thickness *= this.pageViewModel.InverseScale;
            dc.DrawRoundedRectangle(Brushes.Transparent, pen, rectangle, SelectionCornerRadius * this.pageViewModel.InverseScale, SelectionCornerRadius * this.pageViewModel.InverseScale);
        }
        /// <summary>
        /// 渲染事件处理
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(base.RenderSize));
            if (this.startPoint.HasValue && this.endPoint.HasValue)
            {
                this.DrawInnerSelectionRectangle(dc);
                this.DrawOuterSelectionRectangle(dc);
            }
        }
        /// <summary>
        /// 更新选择
        /// </summary>
        private void UpdateSelection()
        {
            this.SetSelectionPreview(false);
            Rect rubberBand = new Rect(this.startPoint.Value, this.endPoint.Value);
            HashSet<ICommonControlViewModel> newSelection = new HashSet<ICommonControlViewModel>(
                from ContentPresenter presenter in this.designerCanvas.Children
                let content = presenter.Content
                where !(content is ISelectionControlViewModel)
                let commonControlViewModel = content as ICommonControlViewModel
                let itemRect = VisualTreeHelper.GetDescendantBounds(presenter)
                let itemBounds = presenter.TransformToAncestor(this.designerCanvas).TransformBounds(itemRect)
                where rubberBand.IntersectsWith(itemBounds)
                select commonControlViewModel);
            this.MergeSelection(newSelection);
            this.SetSelectionPreview(true);
        }
        /// <summary>
        ///	合并选择
        /// </summary>
        /// <param name="newSelection"></param>
        private void MergeSelection(HashSet<ICommonControlViewModel> newSelection)
        {
            this.RemoveUnselectedItems(newSelection);
            this.AddNewSelectedItems(newSelection);
        }
        /// <summary>
        /// 添加新选中项
        /// </summary>
        /// <param name="newSelection"></param>
        private void AddNewSelectedItems(System.Collections.Generic.IEnumerable<ICommonControlViewModel> newSelection)
        {
            foreach (ICommonControlViewModel current in newSelection)
            {
                this.selection.Add(current);
            }
        }
        /// <summary>
        /// 移除未选中项
        /// </summary>
        /// <param name="newSelection"></param>
        private void RemoveUnselectedItems(HashSet<ICommonControlViewModel> newSelection)
        {
            for (int i = this.selection.Count - 1; i >= 0; i--)
            {
                this.RemoveItemIfUnselected(newSelection, i);
            }
        }
        /// <summary>
        /// 如果未选中则移除项
        /// </summary>
        /// <param name="newSelection"></param>
        /// <param name="index"></param>
        private void RemoveItemIfUnselected(HashSet<ICommonControlViewModel> newSelection, int index)
        {
            ICommonControlViewModel item = this.selection[index];
            if (!newSelection.Contains(item))
            {
                this.selection.RemoveAt(index);
            }
            newSelection.Remove(item);
        }
        /// <summary>
        /// 设置选择预览
        /// </summary>
        /// <param name="showPreview"></param>
        private void SetSelectionPreview(bool showPreview)
        {
            foreach (ICommonControlViewModel current in this.selection)
            {
                current.ShowSelectionPreview = (showPreview);
            }
        }
    }
}
