// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace HMI.Infrastructure.Presentation.Behaviors
{
    public static class MultiChildAdornerBehavior
    {
        public static readonly DependencyProperty IsAdornerVisibleProperty = DependencyProperty.RegisterAttached("IsAdornerVisible", typeof(bool), typeof(MultiChildAdornerBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsAdornerVisibleChanged)));
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementMultiChildAdorner), typeof(MultiChildAdornerBehavior));
        public static readonly DependencyProperty AdornerChildrenProperty = DependencyProperty.RegisterAttached("AdornerChildren", typeof(IEnumerable<FrameworkElement>), typeof(MultiChildAdornerBehavior), new PropertyMetadata(new PropertyChangedCallback(OnAdornerChildrenChanged)));
        public static readonly DependencyProperty HorizontalPlacementProperty = DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior), new FrameworkPropertyMetadata(AdornerPlacement.Across));
        public static readonly DependencyProperty VerticalPlacementProperty = DependencyProperty.RegisterAttached("VerticalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior), new FrameworkPropertyMetadata(AdornerPlacement.Across));
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(MultiChildAdornerBehavior), new PropertyMetadata(0.0));
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(MultiChildAdornerBehavior), new PropertyMetadata(0.0));
        public static bool GetIsAdornerVisible(DependencyObject d)
        {
            return (bool)d.GetValue(IsAdornerVisibleProperty);
        }
        public static void SetIsAdornerVisible(DependencyObject d, bool value)
        {
            d.SetValue(IsAdornerVisibleProperty, value);
        }
        public static IEnumerable<FrameworkElement> GetAdornerChildren(DependencyObject d)
        {
            return (IEnumerable<FrameworkElement>)d.GetValue(AdornerChildrenProperty);
        }
        public static void SetAdornerChildren(DependencyObject d, IEnumerable<FrameworkElement> value)
        {
            d.SetValue(AdornerChildrenProperty, value);
        }
        public static AdornerPlacement GetHorizontalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(HorizontalPlacementProperty);
        }
        public static void SetHorizontalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(HorizontalPlacementProperty, value);
        }
        public static AdornerPlacement GetVerticalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(VerticalPlacementProperty);
        }
        public static void SetVerticalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(VerticalPlacementProperty, value);
        }
        public static double GetOffsetX(DependencyObject d)
        {
            return (double)d.GetValue(OffsetXProperty);
        }
        public static void SetOffsetX(DependencyObject d, double value)
        {
            d.SetValue(OffsetXProperty, value);
        }
        public static double GetOffsetY(DependencyObject d)
        {
            return (double)d.GetValue(OffsetYProperty);
        }
        public static void SetOffsetY(DependencyObject d, double value)
        {
            d.SetValue(OffsetYProperty, value);
        }

        static void OnIsAdornerVisibleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
            UpdateAdorner(frameworkElement);
        }
        static void OnAdornerChildrenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
            UpdateAdorner(frameworkElement);
        }
        static void UpdateAdorner(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
            {
                if (GetIsAdornerVisible(frameworkElement))
                {
                    CreateAdorner(frameworkElement);
                    frameworkElement.DataContextChanged += new DependencyPropertyChangedEventHandler(OnElementDataContextChanged);
                    frameworkElement.Loaded += new RoutedEventHandler(OnElementLoaded);
                    return;
                }
                RemoveAdorner(frameworkElement);
                frameworkElement.DataContextChanged -= new DependencyPropertyChangedEventHandler(OnElementDataContextChanged);
            }
        }
        static void RemoveAdorner(FrameworkElement frameworkElement)
        {
            GetAdornerChildren(frameworkElement);
            if (frameworkElement != null && frameworkElement.GetValue(AdornerProperty) != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                if (adornerLayer != null)
                {
                    FrameworkElementMultiChildAdorner adorner = frameworkElement.GetValue(AdornerProperty) as FrameworkElementMultiChildAdorner;
                    if (adorner != null)
                    {
                        adornerLayer.Remove(adorner);
                        adorner.DisconnectChildren();
                        frameworkElement.SetValue(AdornerProperty, null);
                    }
                }
            }
        }
        static void CreateAdorner(FrameworkElement frameworkElement)
        {
            IEnumerable<FrameworkElement> adornerChildren = GetAdornerChildren(frameworkElement);
            if (frameworkElement != null && frameworkElement.GetValue(AdornerProperty) == null && adornerChildren != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                if (adornerLayer != null)
                {
                    FrameworkElementMultiChildAdorner adorner = new FrameworkElementMultiChildAdorner(adornerChildren, frameworkElement);
                    adornerLayer.Add(adorner);
                    BindAdornerChildren(frameworkElement, adorner);
                    frameworkElement.SetValue(AdornerProperty, adorner);
                }
            }
        }
        static void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)sender);
        }
        static void OnElementDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                IEnumerable<FrameworkElement> adornerChildren = GetAdornerChildren(frameworkElement);
                foreach (FrameworkElement adornerChild in adornerChildren)
                {
                    adornerChild.DataContext = frameworkElement.DataContext;
                }
            }
        }
        static void BindAdornerChildren(FrameworkElement frameworkElement, FrameworkElementMultiChildAdorner adorner)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException("element");
            }
            if (adorner == null)
            {
                throw new ArgumentNullException("adorner");
            }
            Binding binding = new Binding
            {
                Path = new PropertyPath(AdornerChildrenProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementMultiChildAdorner.AdornerChildrenProperty, binding);
        }
    }
}
