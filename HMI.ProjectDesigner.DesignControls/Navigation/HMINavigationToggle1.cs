// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.ProjectDesigner.DesignControls.Navigation.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Navigation
{
	public class HMINavigationToggle1 : CommonControl
	{
		static HMINavigationToggle1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINavigationToggle1), new System.Windows.FrameworkPropertyMetadata(typeof(HMINavigationToggle1)));
		}
		public HMINavigationToggle1()
		{
			base.DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.HMINavigationToggle1_DataContextChanged);
		}
		private void HMINavigationToggle1_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			HMINavigationToggle1ViewModel wFNavigationToggle1ViewModel = e.OldValue as HMINavigationToggle1ViewModel;
			if (wFNavigationToggle1ViewModel != null)
			{
				wFNavigationToggle1ViewModel.IsCheckedChanged -= new System.EventHandler<ValueChangedEventArgs<bool>>(this.oldViewModel_IsCheckedChanged);
			}
			HMINavigationToggle1ViewModel wFNavigationToggle1ViewModel2 = e.NewValue as HMINavigationToggle1ViewModel;
			if (wFNavigationToggle1ViewModel2 != null)
			{
				wFNavigationToggle1ViewModel2.IsCheckedChanged += new System.EventHandler<ValueChangedEventArgs<bool>>(this.oldViewModel_IsCheckedChanged);
			}
		}
		private void oldViewModel_IsCheckedChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
			{
				this.SwitchCheckedState("Checked");
				return;
			}
			this.SwitchCheckedState("Unchecked");
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.SwitchCheckedState("Unchecked");
		}
		private void SwitchCheckedState(string state)
		{
			VisualStateSwitcher.SwitchVisualState(this, state, false);
		}
	}
}
