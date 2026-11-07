// SPDX-License-Identifier: GPL-3.0-only
using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.Behaviors
{
    /// <summary>
    /// 框架元素装饰器，用于在WPF中为FrameworkElement添加装饰内容。
    /// </summary>
    public class FrameworkElementAdorner : Adorner
    {
        public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.Register("AdornerContent", typeof(FrameworkElement), typeof(FrameworkElementAdorner));
        public static readonly DependencyProperty HorizontalAdornerPlacementProperty = DependencyProperty.Register("HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(FrameworkElementAdorner), new PropertyMetadata(AdornerPlacement.Inside));
        public static readonly DependencyProperty VerticalAdornerPlacementProperty = DependencyProperty.Register("VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(FrameworkElementAdorner), new PropertyMetadata(AdornerPlacement.Inside));
        public static readonly DependencyProperty AdornerOffsetXProperty = DependencyProperty.Register("AdornerOffsetX", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(0.0));
        public static readonly DependencyProperty AdornerOffsetYProperty = DependencyProperty.Register("AdornerOffsetY", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(0.0));
        private double _positionX = double.NaN;
        private double _positionY = double.NaN;
        public FrameworkElement AdornerContent
        {
            get
            {
                return (FrameworkElement)GetValue(AdornerContentProperty);
            }
            set
            {
                SetValue(AdornerContentProperty, value);
            }
        }
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty);
            }
            set
            {
                SetValue(HorizontalAdornerPlacementProperty, value);
            }
        }
        public AdornerPlacement VerticalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)GetValue(VerticalAdornerPlacementProperty);
            }
            set
            {
                SetValue(VerticalAdornerPlacementProperty, value);
            }
        }
        public double AdornerOffsetX
        {
            get
            {
                return (double)GetValue(AdornerOffsetXProperty);
            }
            set
            {
                SetValue(AdornerOffsetXProperty, value);
            }
        }
        public double AdornerOffsetY
        {
            get
            {
                return (double)GetValue(AdornerOffsetYProperty);
            }
            set
            {
                SetValue(AdornerOffsetYProperty, value);
            }
        }
        public double PositionX
        {
            get
            {
                return _positionX;
            }
            set
            {
                _positionX = value;
            }
        }
        public double PositionY
        {
            get
            {
                return _positionY;
            }
            set
            {
                _positionY = value;
            }
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        protected override IEnumerator LogicalChildren
        {
            get
            {
                return new ArrayList
				{
                    AdornerContent
                }.GetEnumerator();
            }
        }
        public new FrameworkElement AdornedElement
        {
            get
            {
                return (FrameworkElement)base.AdornedElement;
            }
        }
        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            AdornerContent = adornerChildElement;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }
        protected override Size MeasureOverride(Size constraint)
        {
            AdornerContent.Measure(constraint);
            return AdornerContent.DesiredSize;
        }
        /// <summary>
        /// Arranges the content of the adorner.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
		{
			double px = PositionX;
			if (double.IsNaN(px))
			{
				px = calcX();
			}
			double py = PositionY;
			if (double.IsNaN(py))
			{
				py = calcY();
			}
			double width = calcWidth();
			double height = calcHeight();
            AdornerContent.Arrange(new Rect(px, py, width, height));
			return finalSize;
		}
        protected override Visual GetVisualChild(int index)
        {
            return AdornerContent;
        }
        public void DisconnectChild()
        {
            RemoveLogicalChild(AdornerContent);
            RemoveVisualChild(AdornerContent);
        }

        double calcX()
        {
            switch (AdornerContent.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (HorizontalAdornerPlacement == AdornerPlacement.Across)
                    {
                        return -AdornerContent.DesiredSize.Width / 2.0 + AdornerOffsetX;
                    }
                    if (HorizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -AdornerContent.DesiredSize.Width + AdornerOffsetX;
                    }
                    return AdornerOffsetX;
                case HorizontalAlignment.Center:
                    {
                        Size desiredSize = AdornerContent.DesiredSize;
                        return AdornedElement.ActualWidth / 2.0 - desiredSize.Width / 2.0 + AdornerOffsetX;
                    }
                case HorizontalAlignment.Right:
                    {
                        if (HorizontalAdornerPlacement == AdornerPlacement.Across)
                        {
                            return (AdornedElement.ActualWidth - AdornerContent.DesiredSize.Width) / 2.0 + AdornerOffsetX;
                        }
                        if (HorizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return AdornedElement.ActualWidth + AdornerOffsetX;
                        }
                        Size desiredSize2 = AdornerContent.DesiredSize;
                        return AdornedElement.ActualWidth - desiredSize2.Width + AdornerOffsetX;
                    }
                case HorizontalAlignment.Stretch:
                    return 0.0;
                default:
                    return 0.0;
            }
        }
        double calcY()
        {
            switch (AdornerContent.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    if (VerticalAdornerPlacement == AdornerPlacement.Across)
                    {
                        return -AdornerContent.DesiredSize.Height / 2.0 + AdornerOffsetY;
                    }
                    if (VerticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -AdornerContent.DesiredSize.Height + AdornerOffsetY;
                    }
                    return AdornerOffsetY;
                case VerticalAlignment.Center:
                    {
                        Size desiredSize = AdornerContent.DesiredSize;

                        return AdornedElement.ActualHeight / 2.0 - desiredSize.Height / 2.0 + AdornerOffsetY;
                    }
                case VerticalAlignment.Bottom:
                    {
                        if (VerticalAdornerPlacement == AdornerPlacement.Across)
                        {
                            return (AdornedElement.ActualHeight - AdornerContent.DesiredSize.Height) / 2.0 + AdornerOffsetY;
                        }
                        if (VerticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return AdornedElement.ActualHeight + AdornerOffsetY;
                        }
                        Size desiredSize2 = AdornerContent.DesiredSize;
                        return AdornedElement.ActualHeight - desiredSize2.Height + AdornerOffsetY;
                    }
                case VerticalAlignment.Stretch:
                    return 0.0;
                default:
                    return 0.0;
            }
        }
        double calcWidth()
        {
            if (!double.IsNaN(PositionX))
            {
                return AdornerContent.DesiredSize.Width;
            }
            switch (AdornerContent.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return AdornerContent.DesiredSize.Width;
                case HorizontalAlignment.Center:
                    return AdornerContent.DesiredSize.Width;
                case HorizontalAlignment.Right:
                    return AdornerContent.DesiredSize.Width;
                case HorizontalAlignment.Stretch:
                    return AdornedElement.ActualWidth;
                default:
                    return 0.0;
            }
        }
        double calcHeight()
        {
            if (!double.IsNaN(PositionY))
            {
                return AdornerContent.DesiredSize.Height;
            }
            switch (AdornerContent.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return AdornerContent.DesiredSize.Height;
                case VerticalAlignment.Center:
                    return AdornerContent.DesiredSize.Height;
                case VerticalAlignment.Bottom:
                    return AdornerContent.DesiredSize.Height;
                case VerticalAlignment.Stretch:
                    return AdornedElement.ActualHeight;
                default:
                    return 0.0;
            }
        }
    }
}
