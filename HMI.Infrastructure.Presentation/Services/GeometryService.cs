// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 几何服务：提供控件旋转、边界计算、角点计算、多边形命中测试等基础几何算法。
    /// 所有方法均为纯数学计算，不依赖 UI 元素，适合在设计器或运行时使用。
    /// </summary>
    public static class GeometryService
    {
        //private static readonly Logger logger_0 = LogManager.GetLogger("GeometryService");

        /// <summary>
        /// 计算控件在绕某中心点旋转 angleDelta 后的新位置（左上角坐标）。
        /// </summary>
        /// <param name="center">旋转中心点（通常为多选中心或单控件中心）</param>
        /// <param name="control">控件 ViewModel</param>
        /// <param name="angleDelta">旋转角度增量（度）</param>
        /// <returns>旋转后控件左上角的新坐标</returns>
        public static Point GetPositionAfterRotation(Point center, ICommonControlViewModel control, double angleDelta)
        {
            // 控件当前中心点
            Point controlCenter = GeometryService.GetControlCenter(control);
            // 控件中心相对旋转中心的向量
            Vector vector = controlCenter - center;
            // 将向量按增量角度旋转
            Vector vector2 = GeometryService.GetRotationMatrix(angleDelta).Transform(vector);
            // 得到旋转后的控件中心点
            Point center2 = center + vector2;
            // 将中心点转换为左上角坐标
            return GeometryService.GetTranslationRelativeToCenter(center2, control);
        }
        /// <summary>
        /// 获取旋转矩阵（基于 WPF Matrix）。
        /// </summary>
        public static Matrix GetRotationMatrix(double angle)
        {
            Matrix identity = Matrix.Identity;
            identity.Rotate(angle);
            return identity;
        }
        /// <summary>
        /// 获取控件四个角点相对于中心点的向量（已应用旋转）。
        /// 返回顺序：左上、右上、右下、左下。
        /// </summary>
        public static Vector[] GetCenterToCornerVectorsInSurfaceCoordinates(double angle, Size size)
        {
            // 未旋转前的四个角点向量（以中心为原点）
            Vector vector = new Vector(-size.Width / 2.0, -size.Height / 2.0);
            Vector vector2 = new Vector(size.Width / 2.0, -size.Height / 2.0);
            Vector vector3 = new Vector(-size.Width / 2.0, size.Height / 2.0);
            Vector vector4 = new Vector(size.Width / 2.0, size.Height / 2.0);

            Vector[] array = new Vector[]
            {
                vector,
                vector2,
                vector4,
                vector3
            };
            // 应用旋转
            GeometryService.GetRotationMatrix(angle).Transform(array);
            return array;
        }
        /// <summary>
        /// 将 cornerVectors（角点向量）加入到 start-end 边界框中。
        /// 用于计算多个控件的整体包围盒。
        /// </summary>
        public static void IncludeVectorsInBounds(ref Point start, ref Point end, Point center, Vector[] cornerVectors)
        {
            for (int i = 0; i < cornerVectors.Length; i++)
            {
                Vector vector = cornerVectors[i];
                Point point = center + vector;
                // 更新最小/最大边界
                start.X = Math.Min(start.X, point.X);
                start.Y = Math.Min(start.Y, point.Y);
                end.X = Math.Max(end.X, point.X);
                end.Y = Math.Max(end.Y, point.Y);
            }
        }
        /// <summary>
        /// 获取控件中心点（基于控件左上角 + 宽高的一半）。
        /// </summary>
        public static Point GetControlCenter(ICommonControlViewModel control)
        {
            Point result = new Point(control.XTranslation, control.YTranslation);
            result.Offset(control.Width / 2.0, control.Height / 2.0);
            return result;
        }
        /// <summary>
        /// 将中心点转换为控件左上角坐标。
        /// </summary>
        public static Point GetTranslationRelativeToCenter(Point center, ICommonControlViewModel control)
        {
            Point result = center;
            result.Offset(-control.Width / 2.0, -control.Height / 2.0);
            return result;
        }
        /// <summary>
        /// 获取控件中心点与四个角点向量（已旋转）。
        /// </summary>
        public static void GetControlCornerVectors(ICommonControlViewModel control, out Point center, out Vector[] cornerVectors)
        {
            double angle = control.Angle;
            Size size = new Size(control.ControlWidth, control.ControlHeight);
            center = GeometryService.GetControlCenter(control);
            cornerVectors = GeometryService.GetCenterToCornerVectorsInSurfaceCoordinates(angle, size);
        }
        /// <summary>
        /// 获取控件四个角点的绝对坐标（已旋转）。
        /// </summary>
        public static void GetControlCornerPositions(ICommonControlViewModel control, out Point[] cornerPositions)
        {
            Point point;
            Vector[] array;
            GeometryService.GetControlCornerVectors(control, out point, out array);
            cornerPositions = new Point[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                cornerPositions[i] = point + array[i];
            }
        }
        /// <summary>
        /// 获取多个控件的整体包围盒（已考虑旋转）。
        /// </summary>
        public static void GetChildrenBoundsInSurfaceCoordinates(out Point start, out Point end, params ICommonControlViewModel[] items)
        {
            // 初始化为极值，便于后续取 min/max
            start = new Point(double.MaxValue, double.MaxValue);
            end = new Point(double.MinValue, double.MinValue);
            for (int i = 0; i < items.Length; i++)
            {
                ICommonControlViewModel control = items[i];
                Point center;
                Vector[] cornerVectors;
                GeometryService.GetControlCornerVectors(control, out center, out cornerVectors);
                GeometryService.IncludeVectorsInBounds(ref start, ref end, center, cornerVectors);
            }
        }
        /// <summary>
        /// 射线法判断一个点是否在多边形内。
        /// 多边形顶点需按顺序排列（顺时针或逆时针）。
        /// </summary>
        public static bool IsPointInPolygon(Point surfaceCoords, params Point[] polygonTips)
        {
            int len = polygonTips.Length - 1;
            bool inside = false;
            double x1 = surfaceCoords.X;
            double y1 = surfaceCoords.Y;
            int len3 = 0;
            while (len3 < polygonTips.Length)
            {
                double x2 = polygonTips[len3].X;
                double y2 = polygonTips[len3].Y;
                double x3 = polygonTips[len].X;
                double y3 = polygonTips[len].Y;
                // 判断射线是否与边相交
                if (((y2 < y1 && y3 >= y1) || (y3 < y1 && y2 >= y1)) && x2 + (y1 - y2) / (y3 - y2) * (x3 - x2) < x1)
                {
                    // 奇偶原则：交点次数为奇数则在内部
                    inside = (inside == false);
                }
                len = len3;
                len3 = len3 + 1;
            }
            return inside;
        }
    }
}
