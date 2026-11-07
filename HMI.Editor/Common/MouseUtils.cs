// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
namespace HMI.Editor.Common
{

    /// <summary>
    /// 鼠标工具类
    /// </summary>
    public static class MouseUtils
    {
        /// <summary>
        /// 获取相对于指定元素的鼠标坐标
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        public static Point GetMouseCoordinates(Visual relativeTo)
        {
            // 获取鼠标相对于某个元素的当前位置
            Point mousePosition = Mouse.GetPosition(relativeTo as UIElement); // relativeElement可以是任何UIElement
            return mousePosition;

            // 要获取屏幕坐标，可以结合使用PointToScreen
            //Point screenPosition = relativeTo.PointToScreen(mousePosition);
            //return screenPosition;
        }
    }
}
