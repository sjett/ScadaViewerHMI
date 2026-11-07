// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Media;
namespace HMI.Runtime.Shared.Common
{
	public static class VisualTreeHelperWrapper
	{
		public static DependencyObject GetChild(DependencyObject @object, int childIndex)
		{
			if (!(@object is UIElement))
			{
				return null;
			}
			return VisualTreeHelper.GetChild(@object, childIndex);
		}
		public static int GetChildrenCount(DependencyObject @object)
		{
			if (!(@object is UIElement))
			{
				return 0;
			}
			return VisualTreeHelper.GetChildrenCount(@object);
		}
		public static DependencyObject GetParent(DependencyObject @object)
		{
			if (!(@object is UIElement))
			{
				return null;
			}
			return VisualTreeHelper.GetParent(@object);
		}
	}

    public static class VisualTreeExtensions
    {
        public static Rect? GetBoundsRelativeTo(this FrameworkElement element, Visual relativeTo)
        {
            if (element == null || relativeTo == null)
            {
                return null;
            }

            try
            {
                GeneralTransform transform = element.TransformToVisual(relativeTo);
                Rect rect = transform.TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));
                return rect;
            }
            catch
            {
                return null;
            }
        }
    }

}
