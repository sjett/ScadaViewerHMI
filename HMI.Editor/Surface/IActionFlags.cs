// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 设计器节点操作标志接口
    /// </summary>
    public interface IActionFlags
    {
        /// <summary>
        /// 是否启用添加控制点操作
        /// </summary>
        bool IsAddControlPointEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用移动控制点操作
        /// </summary>
        bool IsMoveControlPointEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用移动操作
        /// </summary>
        bool IsMoveEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用调整大小操作
        /// </summary>
        bool IsResizeEnabled
        {
            get;
        }
        /// <summary>
        /// 是否启用旋转操作
        /// </summary>
		bool IsRotateEnabled
        {
            get;
        }
    }
}
