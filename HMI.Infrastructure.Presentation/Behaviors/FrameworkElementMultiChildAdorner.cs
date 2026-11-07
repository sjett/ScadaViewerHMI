// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.Behaviors
{
    /// <summary>
    /// 框架元素多子元素装饰器
    /// </summary>
    public class FrameworkElementMultiChildAdorner : Adorner
    {
        public static readonly DependencyProperty AdornerChildrenProperty = DependencyProperty.Register("AdornerChildren", typeof(IEnumerable<FrameworkElement>), typeof(FrameworkElementMultiChildAdorner));
        public static readonly DependencyProperty AdornedElementProperty = DependencyProperty.RegisterAttached("AdornedElement", typeof(FrameworkElement), typeof(FrameworkElementMultiChildAdorner));
        public static readonly DependencyProperty PositionXProperty = DependencyProperty.RegisterAttached("PositionX", typeof(double), typeof(FrameworkElementMultiChildAdorner), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty PositionYProperty = DependencyProperty.RegisterAttached("PositionY", typeof(double), typeof(FrameworkElementMultiChildAdorner), new PropertyMetadata(double.NaN));
        public IEnumerable<FrameworkElement> AdornerChildren
        {
            get
            {
                return (IEnumerable<FrameworkElement>)GetValue(AdornerChildrenProperty);
            }
            set
            {
                SetValue(AdornerChildrenProperty, value);
            }
        }
        protected override int VisualChildrenCount
        {
            get
            {
                int childCount = 0;
                foreach (FrameworkElement child in AdornerChildren)
                {
                    childCount++;
                }
                return childCount;
            }
        }
        public new FrameworkElement AdornedElement
        {
            get
            {
                return (FrameworkElement)base.AdornedElement;
            }
        }
        public static FrameworkElement GetAdornedElement(DependencyObject d)
        {
            return (FrameworkElement)d.GetValue(AdornedElementProperty);
        }
        public static void SetAdornedElement(DependencyObject d, FrameworkElement value)
        {
            d.SetValue(AdornedElementProperty, value);
        }
        public static double GetPositionX(DependencyObject d)
        {
            return (double)d.GetValue(PositionXProperty);
        }
        public static void SetPositionX(DependencyObject d, double value)
        {
            d.SetValue(PositionXProperty, value);
        }
        public static double GetPositionY(DependencyObject d)
        {
            return (double)d.GetValue(PositionYProperty);
        }
        public static void SetPositionY(DependencyObject d, double value)
        {
            d.SetValue(PositionYProperty, value);
        }
        public FrameworkElementMultiChildAdorner(IEnumerable<FrameworkElement> adornerChildren, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            AdornerChildren = adornerChildren;
            foreach (FrameworkElement child in adornerChildren)
            {
                AddLogicalChild(child);
                AddVisualChild(child);
                SetAdornedElement(child, adornedElement);
            }
            adornedElement.SizeChanged += new SizeChangedEventHandler(OnAdornedElementSizeChanged);
        }
        private void OnAdornedElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }
        protected override Size ArrangeOverride(Size finalSize)
		{
			foreach (FrameworkElement child in AdornerChildren)
			{
				double positionX = GetPositionX(child);
				if (double.IsNaN(positionX))
				{
					positionX = CalculateChildX(child);
				}
				double positionY = GetPositionY(child);
				if (double.IsNaN(positionY))
				{
					positionY = CalculateChildY(child);
				}
				double width = CalculateChildWidth(child);
				double height = CalculateChildHeight(child);
				child.Arrange(new Rect(positionX, positionY, width, height));
			}
			return finalSize;
		}
        protected override Visual GetVisualChild(int index)
        {
            if (AdornerChildren == null)
            {
                return null;
            }
            return new List<FrameworkElement>(AdornerChildren)[index];
        }
        public void DisconnectChildren()
        {
            foreach (FrameworkElement child in AdornerChildren)
            {
                RemoveLogicalChild(child);
                RemoveVisualChild(child);
                SetAdornedElement(child, null);
            }
        }

        double CalculateChildX(FrameworkElement child)
        {
            AdornerPlacement horizontalPlacement = MultiChildAdornerBehavior.GetHorizontalPlacement(child);
            double horizontalOffset = MultiChildAdornerBehavior.GetOffsetX(child);
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (horizontalPlacement == AdornerPlacement.Across)
                    {
                        return -child.DesiredSize.Width / 2.0 + horizontalOffset;
                    }
                    if (horizontalPlacement == AdornerPlacement.Outside)
                    {
                        return -child.DesiredSize.Width + horizontalOffset;
                    }
                    return horizontalOffset;
                case HorizontalAlignment.Center:
                    {
                        Size centeredChildSize = child.DesiredSize;
                        return AdornedElement.ActualWidth / 2.0 - centeredChildSize.Width / 2.0 + horizontalOffset;
                    }
                case HorizontalAlignment.Right:
                    {
                        if (horizontalPlacement == AdornerPlacement.Across)
                        {
                            Size rightAlignedChildSize = child.DesiredSize;
                            return AdornedElement.ActualWidth - rightAlignedChildSize.Width / 2.0 + horizontalOffset;
                        }
                        if (horizontalPlacement == AdornerPlacement.Outside)
                        {
                            return AdornedElement.ActualWidth + horizontalOffset;
                        }
                        Size rightAlignedChildSizeWithoutPlacement = child.DesiredSize;
                        return AdornedElement.ActualWidth - rightAlignedChildSizeWithoutPlacement.Width + horizontalOffset;
                    }
                case HorizontalAlignment.Stretch:
                    return 0.0;
                default:
                    return 0.0;
            }
        }
        double CalculateChildY(FrameworkElement child)
        {
            AdornerPlacement verticalPlacement = MultiChildAdornerBehavior.GetVerticalPlacement(child);
            double verticalOffset = MultiChildAdornerBehavior.GetOffsetY(child);
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    if (verticalPlacement == AdornerPlacement.Across)
                    {
                        return -child.DesiredSize.Height / 2.0 + verticalOffset;
                    }
                    if (verticalPlacement == AdornerPlacement.Outside)
                    {
                        return -child.DesiredSize.Height + verticalOffset;
                    }
                    return verticalOffset;
                case VerticalAlignment.Center:
                    {
                        Size centeredChildSize = child.DesiredSize;
                        return AdornedElement.ActualHeight / 2.0 - centeredChildSize.Height / 2.0 + verticalOffset;
                    }
                case VerticalAlignment.Bottom:
                    {
                        if (verticalPlacement == AdornerPlacement.Across)
                        {
                            Size bottomAlignedChildSize = child.DesiredSize;
                            return AdornedElement.ActualHeight - bottomAlignedChildSize.Height / 2.0 + verticalOffset;
                        }
                        if (verticalPlacement == AdornerPlacement.Outside)
                        {
                            return AdornedElement.ActualHeight + verticalOffset;
                        }
                        Size bottomAlignedChildSizeWithoutPlacement = child.DesiredSize;
                        return AdornedElement.ActualHeight - bottomAlignedChildSizeWithoutPlacement.Height + verticalOffset;
                    }
                case VerticalAlignment.Stretch:
                    return 0.0;
                default:
                    return 0.0;
            }
        }
        double CalculateChildWidth(FrameworkElement child)
        {
            double positionX = GetPositionX(child);
            if (!double.IsNaN(positionX))
            {
                return child.DesiredSize.Width;
            }
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return child.DesiredSize.Width;
                case HorizontalAlignment.Center:
                    return child.DesiredSize.Width;
                case HorizontalAlignment.Right:
                    return child.DesiredSize.Width;
                case HorizontalAlignment.Stretch:
                    return AdornedElement.ActualWidth;
                default:
                    return 0.0;
            }
        }
        double CalculateChildHeight(FrameworkElement child)
        {
            double positionY = GetPositionY(child);
            if (!double.IsNaN(positionY))
            {
                return child.DesiredSize.Height;
            }
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    return child.DesiredSize.Height;
                case VerticalAlignment.Center:
                    return child.DesiredSize.Height;
                case VerticalAlignment.Bottom:
                    return child.DesiredSize.Height;
                case VerticalAlignment.Stretch:
                    return AdornedElement.ActualHeight;
                default:
                    return 0.0;
            }
        }
    }
}
