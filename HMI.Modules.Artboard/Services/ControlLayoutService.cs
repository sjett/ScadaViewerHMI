// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows;

namespace HMI.Modules.Artboard.Services
{
    public class ControlLayoutService : IControlLayoutService
    {
        public void MakeSameWidth(ICommonControlViewModel firstItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            foreach (ICommonControlViewModel item in selectedItems)
            {
                item.ActualWidth = firstItem.ActualWidth;
            }
        }

        public void MakeSameHeight(ICommonControlViewModel firstItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            foreach (ICommonControlViewModel item in selectedItems)
            {
                item.ActualHeight = firstItem.ActualHeight;
            }
        }

        public void AlignLeft(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out _,
                new[] { referenceItem });

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlLeftEdge(item, topLeft.X);
            }
        }

        public void AlignCenter(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out Point bottomRight,
                new[] { referenceItem });
            double centerX = (topLeft.X + bottomRight.X) / 2.0;

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlCenterX(item, centerX);
            }
        }

        public void AlignRight(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out _,
                out Point bottomRight,
                new[] { referenceItem });

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlRightEdge(item, bottomRight.X);
            }
        }

        public void AlignTop(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out _,
                new[] { referenceItem });

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlTopEdge(item, topLeft.Y);
            }
        }

        public void AlignMiddle(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out Point bottomRight,
                new[] { referenceItem });
            double centerY = (topLeft.Y + bottomRight.Y) / 2.0;

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlCenterY(item, centerY);
            }
        }

        public void AlignBottom(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out _,
                out Point bottomRight,
                new[] { referenceItem });

            foreach (ICommonControlViewModel item in selectedItems)
            {
                MoveControlBottomEdge(item, bottomRight.Y);
            }
        }

        public void DistributeHorizontally(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            ICommonControlViewModel[] items = selectedItems.ToArray();
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out Point bottomRight,
                items);

            double spacing = CalculateHorizontalSpacing(items, topLeft, bottomRight);
            DistributeHorizontally(items.OrderBy(GetControlLeft).ToArray(), topLeft.X, spacing);
        }

        public void DistributeVertically(ICommonControlViewModel referenceItem, IEnumerable<ICommonControlViewModel> selectedItems)
        {
            ICommonControlViewModel[] items = selectedItems.ToArray();
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(
                out Point topLeft,
                out Point bottomRight,
                items);

            double spacing = (bottomRight.Y - topLeft.Y - GetTotalHeight(items)) / (items.Length - 1);
            DistributeVertically(items.OrderBy(GetControlTop).ToArray(), topLeft.Y, spacing);
        }

        private static void MoveControlLeftEdge(ICommonControlViewModel control, double left)
        {
            Point[] corners = GetControlCorners(control);
            control.Translate(left - corners.Min(point => point.X), 0.0, TransformMode.Relative);
        }

        private static void MoveControlCenterX(ICommonControlViewModel control, double centerX)
        {
            Point[] corners = GetControlCorners(control);
            double controlCenterX = (corners.Min(point => point.X) + corners.Max(point => point.X)) / 2.0;
            control.Translate(centerX - controlCenterX, 0.0, TransformMode.Relative);
        }

        private static void MoveControlRightEdge(ICommonControlViewModel control, double right)
        {
            Point[] corners = GetControlCorners(control);
            control.Translate(right - corners.Max(point => point.X), 0.0, TransformMode.Relative);
        }

        private static void MoveControlTopEdge(ICommonControlViewModel control, double top)
        {
            Point[] corners = GetControlCorners(control);
            control.Translate(0.0, top - corners.Min(point => point.Y), TransformMode.Relative);
        }

        private static void MoveControlCenterY(ICommonControlViewModel control, double centerY)
        {
            Point[] corners = GetControlCorners(control);
            double controlCenterY = (corners.Min(point => point.Y) + corners.Max(point => point.Y)) / 2.0;
            control.Translate(0.0, centerY - controlCenterY, TransformMode.Relative);
        }

        private static void MoveControlBottomEdge(ICommonControlViewModel control, double bottom)
        {
            Point[] corners = GetControlCorners(control);
            control.Translate(0.0, bottom - corners.Max(point => point.Y), TransformMode.Relative);
        }

        private static void DistributeHorizontally(
            IEnumerable<ICommonControlViewModel> controls,
            double nextLeft,
            double spacing)
        {
            foreach (ICommonControlViewModel control in controls)
            {
                Point[] corners = GetControlCorners(control);
                double left = corners.Min(point => point.X);
                double right = corners.Max(point => point.X);
                control.Translate(nextLeft - left, 0.0, TransformMode.Relative);
                nextLeft += right - left + spacing;
            }
        }

        private static double CalculateHorizontalSpacing(
            IEnumerable<ICommonControlViewModel> controls,
            Point topLeft,
            Point bottomRight)
        {
            ICommonControlViewModel[] controlArray = controls.ToArray();
            return (bottomRight.X - topLeft.X - GetTotalWidth(controlArray)) / (controlArray.Length - 1);
        }

        private static double GetTotalWidth(IEnumerable<ICommonControlViewModel> controls)
        {
            return controls.Sum(control =>
            {
                Point[] corners = GetControlCorners(control);
                return corners.Max(point => point.X) - corners.Min(point => point.X);
            });
        }

        private static void DistributeVertically(
            IEnumerable<ICommonControlViewModel> controls,
            double nextTop,
            double spacing)
        {
            foreach (ICommonControlViewModel control in controls)
            {
                Point[] corners = GetControlCorners(control);
                double top = corners.Min(point => point.Y);
                double bottom = corners.Max(point => point.Y);
                control.Translate(0.0, nextTop - top, TransformMode.Relative);
                nextTop += bottom - top + spacing;
            }
        }

        private static double GetTotalHeight(IEnumerable<ICommonControlViewModel> controls)
        {
            return controls.Sum(control =>
            {
                Point[] corners = GetControlCorners(control);
                return corners.Max(point => point.Y) - corners.Min(point => point.Y);
            });
        }

        private static double GetControlLeft(ICommonControlViewModel control)
        {
            return GetControlCorners(control).Min(point => point.X);
        }

        private static double GetControlTop(ICommonControlViewModel control)
        {
            return GetControlCorners(control).Min(point => point.Y);
        }

        private static Point[] GetControlCorners(ICommonControlViewModel control)
        {
            GeometryService.GetControlCornerPositions(control, out Point[] corners);
            return corners;
        }
    }
}
