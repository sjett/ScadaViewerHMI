// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Editor.Surface
{
    /// <summary>
    /// 选择操作
    /// </summary>
    public class SelectAction : DesignerAction
    {
        /// <summary>
        /// 选择阈值距离
        /// </summary>
        public const double SelectThresholdDistance = 3.0;
        private Point? rubberbandSelectionStartPoint = null;

        /// <summary>
        /// 选择操作构造函数
        /// </summary>
        /// <param name="designer"></param>
        public SelectAction(DesignerCanvas designer) : base(designer)
        {
        }

        /// <summary>
        /// 获取页面视图模型
        /// </summary>
        public IProjectPageViewModel ViewModel
        {
            get
            {
                return base.Designer.DataContext as IProjectPageViewModel;
            }
        }
        /// <summary>
        /// 是否启用调整大小功能
        /// </summary>
        public override bool IsResizeEnabled
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 是否启用旋转功能
        /// </summary>
        public override bool IsRotateEnabled
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 是否启用添加控制点功能
        /// </summary>
        public override bool IsAddControlPointEnabled
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 是否启用移动控制点功能
        /// </summary>
        public override bool IsMoveControlPointEnabled
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 是否启用移动功能
        /// </summary>
        public override bool IsMoveEnabled
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            this.rubberbandSelectionStartPoint = new Point?(e.GetPosition(base.Designer));
            e.Handled = true;
        }
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.rubberbandSelectionStartPoint = null;
            }
            if (this.rubberbandSelectionStartPoint.HasValue)
            {
                Point position = e.GetPosition(base.Designer);
                double length = (position - this.rubberbandSelectionStartPoint.Value).Length;
                if (length > 3.0)
                {
                    base.Designer.Focus();
                    Keyboard.Focus(base.Designer);
                    this.AddSelectionAdorner();
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 添加选择框装饰器
        /// </summary>
        private void AddSelectionAdorner()
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(base.Designer);
            if (adornerLayer != null)
            {
                RubberbandAdorner adorner = new RubberbandAdorner(base.Designer, new Point?(this.rubberbandSelectionStartPoint.GetValueOrDefault()), this.ViewModel);
                adornerLayer.Add(adorner);
            }
        }
    }
}
