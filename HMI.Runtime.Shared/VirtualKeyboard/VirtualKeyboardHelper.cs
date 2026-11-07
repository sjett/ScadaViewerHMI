// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class VirtualKeyboardHelper : IVirtualKeyboardHelper
    {
        public void CenterBottomControl(FrameworkElement element, FrameworkElement parent, double zoomFactor)
        {
            double availableHeight = parent.Height / zoomFactor;
            double availableWidth = parent.Width / zoomFactor;
            if ((element.ActualHeight == 0.0) || double.IsNaN(element.ActualHeight))
            {
                element.Measure(new Size(availableWidth, availableHeight));
            }
            element.SetValue(Canvas.TopProperty, Math.Ceiling((double) (availableHeight - element.DesiredSize.Height)));
            element.SetValue(Canvas.LeftProperty, Math.Ceiling((double) ((availableWidth - element.DesiredSize.Width) / 2.0)));
        }

        private Matrix CreateMatrix(GeneralTransform gt)
        {
            Point origin = gt.Transform(new Point(0.0, 0.0));
            Point unitXPoint = gt.Transform(new Point(1.0, 0.0));
            Point unitYPoint = gt.Transform(new Point(0.0, 1.0));
            Matrix transformMatrix = Matrix.Identity;
            transformMatrix.M11 = unitXPoint.X - origin.X;
            transformMatrix.M12 = unitXPoint.Y - origin.Y;
            transformMatrix.M21 = unitYPoint.X - origin.X;
            transformMatrix.M22 = unitYPoint.Y - origin.Y;
            transformMatrix.OffsetX = origin.X;
            transformMatrix.OffsetY = origin.Y;
            this.InvertMatrix(ref transformMatrix);
            return transformMatrix;
        }

        public Transform CreateRenderTransform(GeneralTransform transformToVisual)
        {
            Matrix renderMatrix = this.CreateMatrix(transformToVisual);
            if (renderMatrix.IsIdentity)
            {
                return null;
            }
            MatrixTransform transform = new MatrixTransform();
            transform.Matrix = renderMatrix;
            return transform;
        }

        private bool InvertMatrix(ref Matrix matrix)
        {
            double determinant = (matrix.M11 * matrix.M22) - (matrix.M12 * matrix.M21);
            if (determinant == 0.0)
            {
                return false;
            }
            Matrix originalMatrix = matrix;
            matrix.M11 = originalMatrix.M22 / determinant;
            matrix.M12 = -originalMatrix.M12 / determinant;
            matrix.M21 = -originalMatrix.M21 / determinant;
            matrix.M22 = originalMatrix.M11 / determinant;
            matrix.OffsetX = ((originalMatrix.OffsetY * originalMatrix.M21) - (originalMatrix.OffsetX * originalMatrix.M22)) / determinant;
            matrix.OffsetY = ((originalMatrix.OffsetX * originalMatrix.M12) - (originalMatrix.OffsetY * originalMatrix.M11)) / determinant;
            return true;
        }
    }
}
