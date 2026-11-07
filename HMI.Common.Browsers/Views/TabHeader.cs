// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
namespace HMI.Common.Browsers.Views
{
    public static class TabHeader
    {
        public static readonly DependencyProperty VisibilityProperty = 
            DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), 
                typeof(TabHeader), new PropertyMetadata(new PropertyChangedCallback(OnVisibilityChanged)));
        public static Visibility GetVisibility(DependencyObject obj)
        {
            return (Visibility)obj.GetValue(TabHeader.VisibilityProperty);
        }
        public static void SetVisibility(DependencyObject obj, Visibility value)
        {
            obj.SetValue(TabHeader.VisibilityProperty, value);
        }

        static void OnVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
        {
            RadTabControl radTabControl = dependencyObject as RadTabControl;
            if (radTabControl == null)
            {
                return;
            }
            Visibility visibility = (Visibility)change.NewValue;
            if (radTabControl.IsLoaded)
            {
                ApplyTabHeaderVisibility(radTabControl, visibility);
                return;
            }
            ApplyVisibilityWhenLoaded(radTabControl, visibility);
        }

        static RoutedEventHandler onLoadedHandler;
        static void ApplyVisibilityWhenLoaded(RadTabControl tabControl, Visibility visibility)
        {
            onLoadedHandler = (object sender, RoutedEventArgs e) => {
               ((RadTabControl)sender).Loaded -= onLoadedHandler;
               ApplyTabHeaderVisibility((RadTabControl)sender, visibility);
           };

            tabControl.Loaded += onLoadedHandler;
        }
        static void ApplyTabHeaderVisibility(RadTabControl tabControl, Visibility visibility)
        {
            LayoutTransformControl layoutTransformControl = FindLayoutTransformControl(tabControl);
            if (layoutTransformControl != null)
            {
                layoutTransformControl.Visibility = visibility;
                Border border = FindFirstBorder(layoutTransformControl.Parent as Panel);
                if (border != null)
                {
                    border.BorderThickness = ((visibility == Visibility.Visible) ? new Thickness(1.0, 0.0, 1.0, 1.0) : new Thickness(0.0));
                    border.Margin = ((visibility == Visibility.Visible) ? new Thickness(0.0) : new Thickness(3.0));
                }
            }
        }
        static LayoutTransformControl FindLayoutTransformControl(RadTabControl tabControl)
        {
            return tabControl.FindChildByType<LayoutTransformControl>();
        }
        static Border FindFirstBorder(Panel panel)
        {
            if (panel == null)
            {
                return null;
            }
            return panel.Children.OfType<Border>().FirstOrDefault<Border>();
        }
    }
}
