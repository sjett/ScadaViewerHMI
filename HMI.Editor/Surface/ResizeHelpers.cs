// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 调整大小的帮助类
    /// </summary>
    public static class ResizeHelpers
	{
        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="item">控件</param>
        /// <param name="relativeTo">关联依赖容器</param>
        /// <param name="horizontalAlignment">水平对齐</param>
        /// <param name="verticalAlignment">垂直对齐</param>
        /// <param name="horizontalDelta">水平移动量</param>
        /// <param name="verticalDelta">垂直移动量</param>
        /// <param name="transformOrigin">中心点</param>
        /// <param name="angle">旋转角度</param>
        public static void Resize(Control item, Control relativeTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, double horizontalDelta, double verticalDelta, Point transformOrigin, double angle)
		{
            UpdateWidth(item);
            UpdateHeight(item);
            // 计算水平和垂直的变化量，考虑旋转和中心点
            double horizontalChange = horizontalDelta * Math.Max(item.ActualWidth, 1.0) / Math.Max(relativeTo.ActualWidth, 1.0);
			double verticalChange = verticalDelta * Math.Max(item.ActualHeight, 1.0) / Math.Max(relativeTo.ActualHeight, 1.0);
            ResizeVertically(item, verticalChange, verticalAlignment, angle, transformOrigin);
            ResizeHorizontally(item, horizontalChange, horizontalAlignment, angle, transformOrigin);
		}
		public static void UpdateHeight(Control item)
		{
			if (double.IsNaN(item.Height))
			{
				item.Height = item.ActualHeight;
			}
		}
		public static void UpdateWidth(Control item)
		{
			if (double.IsNaN(item.Width))
			{
				item.Width = item.ActualWidth;
			}
		}
		/// <summary>
		/// 调整水平大小
		/// </summary>
		/// <param name="item"></param>
		/// <param name="horizontalChange"></param>
		/// <param name="horizontalAlignment"></param>
		/// <param name="angle"></param>
		/// <param name="transformOrigin"></param>
		public static void ResizeHorizontally(Control item, double horizontalChange, HorizontalAlignment horizontalAlignment, double angle, Point transformOrigin)
		{
			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Left:
					{
                        // 计算实际的水平变化量，确保不小于最小宽度
                        double num = Math.Min(horizontalChange, item.ActualWidth - item.MinWidth);
						Canvas.SetTop(item, Canvas.GetTop(item) + num * Math.Sin(angle) - transformOrigin.X * num * Math.Sin(angle));// 根据旋转角度和中心点调整垂直位置
                        Canvas.SetLeft(item, Canvas.GetLeft(item) + num * Math.Cos(angle) + transformOrigin.X * num * (1.0 - Math.Cos(angle)));// 根据旋转角度和中心点调整水平位置
                        item.Width -= num;
						return;
					}
				case HorizontalAlignment.Center:
					break;
				case HorizontalAlignment.Right:
					{
						double num = Math.Min(-horizontalChange, item.ActualWidth - item.MinWidth);// 计算实际的水平变化量，确保不小于最小宽度
                        Canvas.SetTop(item, Canvas.GetTop(item) - transformOrigin.X * num * Math.Sin(angle));// 根据旋转角度和中心点调整垂直位置
                        Canvas.SetLeft(item, Canvas.GetLeft(item) + num * transformOrigin.X * (1.0 - Math.Cos(angle)));// 根据旋转角度和中心点调整水平位置
                        item.Width -= num;
						break;
					}
				default:
					return;
			}
		}
		public static void ResizeVertically(Control item, double verticalChange, VerticalAlignment verticalAlignment, double angle, Point transformOrigin)
		{
			switch (verticalAlignment)
			{
				case VerticalAlignment.Top:
					{
						double num = Math.Min(verticalChange, item.ActualHeight - item.MinHeight);// 计算实际的垂直变化量，确保不小于最小高度
                        Canvas.SetTop(item, Canvas.GetTop(item) + num * Math.Cos(-angle) + transformOrigin.Y * num * (1.0 - Math.Cos(-angle)));// 根据旋转角度和中心点调整垂直位置
                        Canvas.SetLeft(item, Canvas.GetLeft(item) + num * Math.Sin(-angle) - transformOrigin.Y * num * Math.Sin(-angle));// 根据旋转角度和中心点调整水平位置
                        item.Height -= num;
						break;
					}
				case VerticalAlignment.Center:
					break;
				case VerticalAlignment.Bottom:
					{
						double num = Math.Min(-verticalChange, item.ActualHeight - item.MinHeight);// 计算实际的垂直变化量，确保不小于最小高度
                        Canvas.SetTop(item, Canvas.GetTop(item) + transformOrigin.Y * num * (1.0 - Math.Cos(-angle)));// 根据旋转角度和中心点调整垂直位置
                        Canvas.SetLeft(item, Canvas.GetLeft(item) - num * transformOrigin.Y * Math.Sin(-angle));//	根据旋转角度和中心点调整水平位置
                        item.Height -= num;
						return;
					}
				default:
					return;
			}
		}
        /// <summary>
        /// 获取旋转角度（以弧度为单位）
        /// </summary>
        /// <param name="rotateTransform"></param>
        /// <returns></returns>
        public static double GetRotationAngle(RotateTransform rotateTransform)
		{
			double result = 0.0;
			if (rotateTransform != null)
			{
				result = rotateTransform.Angle * Math.PI / 180.0;
			}
			return result;
		}
	}
}
