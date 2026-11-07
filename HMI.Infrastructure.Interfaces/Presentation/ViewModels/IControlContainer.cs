// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 控件容器接口
    /// </summary>
    public interface IControlContainer : IInputElement
    {
        /// <summary>
        /// 内容
        /// </summary>
        object Content
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为组合控件
        /// </summary>
        bool IsGroup
        {
            get;
        }
        /// <summary>
        /// 更新布局
        /// </summary>
        void UpdateLayout();
        /// <summary>
        /// 获取内边界矩形
        /// </summary>
        /// <returns></returns>
        Rect GetInnerBoundingRect();
        /// <summary>
        /// 获取外边界矩形
        /// </summary>
        /// <returns></returns>
        Rect GetOuterBoundingRect();
        /// <summary>
        /// 转换到祖先元素
        /// </summary>
        /// <param name="ancestor">祖先元素</param>
        /// <returns></returns>
        GeneralTransform TransformToAncestor(Visual ancestor);
        /// <summary>
        /// 转换到后代元素
        /// </summary>
        /// <param name="descendant">后代元素</param>
        /// <returns></returns>
        GeneralTransform TransformToDescendant(Visual descendant);
        /// <summary>
        /// 转换到指定元素
        /// </summary>
        /// <param name="visual">元素</param>
        /// <returns></returns>
        GeneralTransform TransformToVisual(Visual visual);
        /// <summary>
        /// 转换控件点到指定元素
        /// </summary>
        /// <param name="point">控件点</param>
        /// <param name="relativeTo">指定元素</param>
        /// <returns>新的点</returns>
        Point TranslateControlPoint(Point point, UIElement relativeTo);
    }
}
