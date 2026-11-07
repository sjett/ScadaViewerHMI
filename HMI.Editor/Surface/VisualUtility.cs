// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 视觉工具类
    /// </summary>
    public class VisualUtility
    {
        /// <summary>
        /// 从视觉对象创建位图
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="visualToRender">视觉对象</param>
        /// <param name="undoTransformation">撤消转换</param>
        /// <returns></returns>
        public static BitmapSource CreateBitmapFromVisual(double width, double height, Visual visualToRender, bool undoTransformation)
        {
            if (visualToRender == null)
            {
                return null;
            }
            var dipScale = VisualTreeHelper.GetDpi(visualToRender);
            // 创建RenderTargetBitmap
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)Math.Ceiling(width),
                (int)Math.Ceiling(height),
                dipScale.PixelsPerInchX,//(double)DeviceHelper.PixelsPerInch(Orientation.Horizontal), 
                dipScale.PixelsPerInchY,//(double)DeviceHelper.PixelsPerInch(Orientation.Vertical), 
                PixelFormats.Pbgra32);

            if (undoTransformation)
            {
                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    VisualBrush brush = new VisualBrush(visualToRender);
                    drawingContext.DrawRectangle(brush, null, new Rect(default(Point), new Size(width, height)));
                }
                renderTargetBitmap.Render(drawingVisual);
            }
            else
            {
                renderTargetBitmap.Render(visualToRender);
            }
            return renderTargetBitmap;
        }
    }

}
