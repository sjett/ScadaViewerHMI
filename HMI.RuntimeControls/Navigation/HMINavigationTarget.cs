// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.RuntimeControls.Navigation
{
	public class HMINavigationTarget : ContentControl
	{
		private bool isLoaded;

		public static readonly DependencyProperty DefaultPageProperty = DependencyProperty.Register("DefaultPage", typeof(string), typeof(HMINavigationTarget), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigationTarget.OnDefaultPagePropertyChanged)));
		public static readonly DependencyProperty StretchContentProperty = DependencyProperty.Register("StretchContent", typeof(Stretch), typeof(HMINavigationTarget), new PropertyMetadata(Stretch.None));
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(HMINavigationTarget), new PropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(HMINavigationTarget.OnHorizontalScrollBarVisibilityPropertyChanged)));
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(HMINavigationTarget), new PropertyMetadata(ScrollBarVisibility.Auto, new PropertyChangedCallback(HMINavigationTarget.OnVerticalScrollBarVisibilityPropertyChanged)));
		
		[Category("HMI")]
		public string DefaultPage
		{
			get
			{
				return (string)base.GetValue(HMINavigationTarget.DefaultPageProperty);
			}
			set
			{
				base.SetValue(HMINavigationTarget.DefaultPageProperty, value);
			}
		}
		[Category("HMI")]
		public Stretch StretchContent
		{
			get
			{
				return (Stretch)base.GetValue(HMINavigationTarget.StretchContentProperty);
			}
			set
			{
				base.SetValue(HMINavigationTarget.StretchContentProperty, value);
			}
		}
		[Category("HMI")]
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(HMINavigationTarget.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(HMINavigationTarget.HorizontalScrollBarVisibilityProperty, value);
			}
		}
		[Category("HMI")]
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(HMINavigationTarget.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(HMINavigationTarget.VerticalScrollBarVisibilityProperty, value);
			}
		}
		private static void OnDefaultPagePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigationTarget? wFNavigationTarget = dp as HMINavigationTarget;
			if (wFNavigationTarget == null || !wFNavigationTarget.isLoaded)
			{
				return;
			}
			string typeName = (string)e.NewValue;
			if (string.IsNullOrEmpty(typeName))
			{
				return;
			}
			UserControl? userControl = (UserControl?)Application.Current.GetType().Assembly.CreateInstance(typeName);
			if (userControl != null)
			{
				wFNavigationTarget.Content=(userControl);
				return;
			}
			wFNavigationTarget.Content=(typeName);
		}
		private static void OnHorizontalScrollBarVisibilityPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigationTarget? wFNavigationTarget = dp as HMINavigationTarget;
			if (wFNavigationTarget == null)
			{
				return;
			}
			wFNavigationTarget.SetScrollBarVisibility();
		}
		private static void OnVerticalScrollBarVisibilityPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigationTarget? wFNavigationTarget = dp as HMINavigationTarget;
			if (wFNavigationTarget == null)
			{
				return;
			}
			wFNavigationTarget.SetScrollBarVisibility();
		}
		public HMINavigationTarget()
		{
			base.DefaultStyleKey=(typeof(HMINavigationTarget));
			base.Loaded+=(new RoutedEventHandler(this.HMINavigationTarget_Loaded));
		}
		private void HMINavigationTarget_Loaded(object sender, RoutedEventArgs e)
		{
			this.isLoaded = true;
			if (string.IsNullOrEmpty(this.DefaultPage))
			{
				return;
			}
			UserControl? userControl = (UserControl)Application.Current.GetType().Assembly.CreateInstance(this.DefaultPage);
			if (userControl == null)
			{
				return;
			}
			base.Content=(userControl);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.SetScrollBarVisibility();
		}
		private void SetScrollBarVisibility()
		{
			base.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, this.HorizontalScrollBarVisibility);
			base.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, this.VerticalScrollBarVisibility);
		}
	}
}
