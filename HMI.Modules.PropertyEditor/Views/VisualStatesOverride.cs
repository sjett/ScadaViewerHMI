// SPDX-License-Identifier: GPL-3.0-only
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI.Modules.PropertyEditor.Views
{
	public static class VisualStatesOverride
	{
		public static readonly DependencyProperty ClearVisualStatesProperty = 
			DependencyProperty.RegisterAttached("ClearVisualStates", typeof(bool), typeof(VisualStatesOverride), 
				new PropertyMetadata(false, new PropertyChangedCallback(OnClearVisualStatesChanged)));
		public static void SetClearVisualStates(FrameworkElement element, bool value)
		{
			element.SetValue(ClearVisualStatesProperty, value);
		}
		public static bool GetClearVisualStates(FrameworkElement element)
		{
			return (bool)element.GetValue(ClearVisualStatesProperty);
		}
        private static void OnClearVisualStatesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var frameworkElement = dependencyObject as FrameworkElement;

                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    addHandler => frameworkElement.Loaded += addHandler, 
                    removeHandler => frameworkElement.Loaded -= removeHandler)
                    .Take(1)
                    .Do(e =>
                    {
                        FrameworkElement rootElement = VisualTreeHelper.GetChild(frameworkElement, 0) as FrameworkElement;
                        if (rootElement == null)
                        {
                            return;
                        }
                        var visualStateGroups = VisualStateManager.GetVisualStateGroups(rootElement);
                        if (visualStateGroups != null)
                        {
                            VisualStateManager.GoToState(frameworkElement, "Normal", false);
                            visualStateGroups.Clear();
                        }
                    })
                    .Subscribe<EventPattern<RoutedEventArgs>>();
            }
        }
    }
}
