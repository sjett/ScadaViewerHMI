// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace HMI.Infrastructure.Presentation.Behaviors
{
    /// <summary>
    /// 装饰器位置枚举
    /// </summary>
    public enum AdornerPlacement
    {
        /// <summary>
        /// 跨越
        /// </summary>
        Across,
        /// <summary>
        /// 内部
        /// </summary>
        Inside,
        /// <summary>
        /// 外部
        /// </summary>
        Outside
    }
    /// <summary>
    ///  装饰器行为，用于在框架元素上显示/隐藏装饰器。
    /// </summary>
    public static class AdornerBehavior
    {
        public static readonly DependencyProperty IsAdornerVisibleProperty = DependencyProperty.RegisterAttached("IsAdornerVisible", typeof(bool), typeof(AdornerBehavior), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsAdornerVisibleChanged)));
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementAdorner), typeof(AdornerBehavior));
        public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.RegisterAttached("AdornerContent", typeof(FrameworkElement), typeof(AdornerBehavior), new PropertyMetadata(new PropertyChangedCallback(OnAdornerContentChanged)));
        public static readonly DependencyProperty HorizontalAdornerPlacementProperty = DependencyProperty.RegisterAttached("HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior), new FrameworkPropertyMetadata(AdornerPlacement.Inside));
        public static readonly DependencyProperty VerticalAdornerPlacementProperty = DependencyProperty.RegisterAttached("VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior), new FrameworkPropertyMetadata(AdornerPlacement.Inside));
        public static readonly DependencyProperty AdornerOffsetXProperty = DependencyProperty.RegisterAttached("AdornerOffsetX", typeof(double), typeof(AdornerBehavior));
        public static readonly DependencyProperty AdornerOffsetYProperty = DependencyProperty.RegisterAttached("AdornerOffsetY", typeof(double), typeof(AdornerBehavior));
        public static bool GetIsAdornerVisible(DependencyObject d)
        {
            return (bool)d.GetValue(IsAdornerVisibleProperty);
        }
        public static void SetIsAdornerVisible(DependencyObject d, bool value)
        {
            d.SetValue(IsAdornerVisibleProperty, value);
        }
        public static FrameworkElement GetAdornerContent(DependencyObject d)
        {
            return (FrameworkElement)d.GetValue(AdornerContentProperty);
        }
        public static void SetAdornerContent(DependencyObject d, FrameworkElement value)
        {
            d.SetValue(AdornerContentProperty, value);
        }
        public static AdornerPlacement GetHorizontalAdornerPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(HorizontalAdornerPlacementProperty);
        }
        public static void SetHorizontalAdornerPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(HorizontalAdornerPlacementProperty, value);
        }
        public static AdornerPlacement GetVerticalAdornerPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(VerticalAdornerPlacementProperty);
        }
        public static void SetVerticalAdornerPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(VerticalAdornerPlacementProperty, value);
        }
        public static double GetAdornerOffsetX(DependencyObject d)
        {
            return (double)d.GetValue(AdornerOffsetXProperty);
        }
        public static void SetAdornerOffsetX(DependencyObject d, double value)
        {
            d.SetValue(AdornerOffsetXProperty, value);
        }
        public static double GetAdornerOffsetY(DependencyObject d)
        {
            return (double)d.GetValue(AdornerOffsetYProperty);
        }
        public static void SetAdornerOffsetY(DependencyObject d, double value)
        {
            d.SetValue(AdornerOffsetYProperty, value);
        }
        static void OnAdornerContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
            UpdateAdorner(frameworkElement);
        }
        static void OnIsAdornerVisibleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
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
                    frameworkElement.DataContextChanged += new DependencyPropertyChangedEventHandler(UpdateAdornerDataContext);
                    frameworkElement.Loaded += new RoutedEventHandler(OnAdornedElementLoaded);
                    return;
                }
                RemoveAdorner(frameworkElement);
                frameworkElement.DataContextChanged -= new DependencyPropertyChangedEventHandler(UpdateAdornerDataContext);
            }
        }
        static void UpdateAdornerDataContext(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement adornedElement = sender as FrameworkElement;
            if (adornedElement != null)
            {
                FrameworkElement adornerContent = GetAdornerContent(adornedElement);
                if (adornerContent != null)
                {
                    adornerContent.DataContext = adornedElement.DataContext;
                }
            }
        }
        static void OnAdornedElementLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)sender);
        }
        static void RemoveAdorner(FrameworkElement frameworkElement)
        {
            GetAdornerContent(frameworkElement);
            if (frameworkElement != null && frameworkElement.GetValue(AdornerProperty) != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                if (adornerLayer != null)
                {
                    FrameworkElementAdorner adorner = frameworkElement.GetValue(AdornerProperty) as FrameworkElementAdorner;
                    if (adorner != null)
                    {
                        adornerLayer.Remove(adorner);
                        adorner.DisconnectChild();
                        frameworkElement.SetValue(AdornerProperty, null);
                    }
                }
            }
        }
        static void CreateAdorner(FrameworkElement frameworkElement)
        {
            FrameworkElement adornerContent = GetAdornerContent(frameworkElement);
            if (frameworkElement != null && frameworkElement.GetValue(AdornerProperty) == null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                if (adornerLayer != null)
                {
                    FrameworkElementAdorner adorner = new FrameworkElementAdorner(adornerContent, frameworkElement);
                    adornerLayer.Add(adorner);
                    BindAdornerProperties(frameworkElement, adorner);
                    frameworkElement.SetValue(AdornerProperty, adorner);
                }
            }
        }
        static void BindAdornerProperties(FrameworkElement frameworkElement, FrameworkElementAdorner adorner)
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
                Path = new PropertyPath(AdornerContentProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementAdorner.AdornerContentProperty, binding);
            binding = new Binding
            {
                Path = new PropertyPath(HorizontalAdornerPlacementProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementAdorner.HorizontalAdornerPlacementProperty, binding);
            binding = new Binding
            {
                Path = new PropertyPath(VerticalAdornerPlacementProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementAdorner.VerticalAdornerPlacementProperty, binding);
            binding = new Binding
            {
                Path = new PropertyPath(AdornerOffsetXProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementAdorner.AdornerOffsetXProperty, binding);
            binding = new Binding
            {
                Path = new PropertyPath(AdornerOffsetYProperty)
            };
            binding.Mode = BindingMode.OneWay;
            binding.Source = frameworkElement;
            adorner.SetBinding(FrameworkElementAdorner.AdornerOffsetYProperty, binding);
        }
    }
}
