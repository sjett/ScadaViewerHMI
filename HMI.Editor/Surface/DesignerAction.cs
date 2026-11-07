// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Input;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 设计器操作基类
    /// </summary>
    public abstract class DesignerAction : IActionFlags
    {
        /// <summary>
        /// 设计器画布
        /// </summary>
        private DesignerCanvas designer;

        /// <summary>
        /// 设计器操作基类构造函数
        /// </summary>
        /// <param name="designer"></param>
        public DesignerAction(DesignerCanvas designer)
        {
            this.designer = designer;
        }

        /// <summary>
        /// 获取设计器画布
        /// </summary>
        public DesignerCanvas Designer
        {
            get
            {
                return this.designer;
            }
        }
        /// <summary>
        /// 是否启用移动功能
        /// </summary>
        public abstract bool IsMoveEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用调整大小功能
        /// </summary>
        public abstract bool IsResizeEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用旋转功能
        /// </summary>
        public abstract bool IsRotateEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用添加控制点功能
        /// </summary>
        public abstract bool IsAddControlPointEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用移动控制点功能
        /// </summary>
        public abstract bool IsMoveControlPointEnabled
        {
            get;
        }
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnMouseDown(MouseButtonEventArgs e)
        {
        }
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnMouseMove(MouseEventArgs e)
        {
        }
        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnMouseUp(MouseButtonEventArgs e)
        {
        }
    }
}
