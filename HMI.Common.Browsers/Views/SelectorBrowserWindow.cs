// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
namespace HMI.Common.Browsers.Views
{
    public class SelectorBrowserWindow : BrowserWindow
    {
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(string), typeof(SelectorBrowserWindow), null);
        public string SelectedValue
        {
            get
            {
                return (string)base.GetValue(SelectorBrowserWindow.SelectedValueProperty);
            }
            set
            {
                base.SetValue(SelectorBrowserWindow.SelectedValueProperty, value);
            }
        }
        public SelectorBrowserWindow()
        {
        }
        public SelectorBrowserWindow(ISelectorBrowserViewModel viewModel)
            : base(viewModel)
		{
			base.SetBinding(SelectorBrowserWindow.SelectedValueProperty, new Binding
			{
				Source = viewModel,
				Path = new PropertyPath("SelectedValue", new object[0]),
				Mode = BindingMode.TwoWay
			});
			Action<RoutedEventHandler> addLoadedHandler = delegate(RoutedEventHandler handler)
			{
				base.Loaded += handler;
			};
			Action<RoutedEventHandler> removeLoadedHandler = delegate(RoutedEventHandler handler)
			{
				base.Loaded -= handler;
			};

			var loadedEvents = Observable
				.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(addLoadedHandler, removeLoadedHandler)
				.Take(1);

			Action<EventPattern<RoutedEventArgs>> activateFirstVisiblePage = delegate(EventPattern<RoutedEventArgs> pattern)
			{
				viewModel.ActivePage = viewModel.Pages.FirstOrDefault((ISelectorBrowserPageViewModel x) => x.IsVisible);
			};
			loadedEvents
				.Do(activateFirstVisiblePage)
				.Subscribe<EventPattern<RoutedEventArgs>>();
		}
    }
}
