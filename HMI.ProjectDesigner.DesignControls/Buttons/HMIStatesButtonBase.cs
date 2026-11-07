// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIStatesButtonBase : CommonControl, System.IDisposable
	{
		private bool isDisposed;
		static HMIStatesButtonBase()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIStatesButtonBase), new System.Windows.FrameworkPropertyMetadata(typeof(HMIStatesButtonBase)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			HMIStatesButtonBaseViewModel wFStatesButtonBaseViewModel = base.DataContext as HMIStatesButtonBaseViewModel;
			if (wFStatesButtonBaseViewModel == null)
			{
				return;
			}
			this.UpdateIconPlacement(wFStatesButtonBaseViewModel.IconPlacement);
			wFStatesButtonBaseViewModel.IconPlacementChanged += new System.EventHandler<ValueChangedEventArgs<ButtonIconPlacement>>(this.viewModel_IconPlacementChanged);
		}
		private void viewModel_IconPlacementChanged(object sender, ValueChangedEventArgs<ButtonIconPlacement> e)
		{
			this.UpdateIconPlacement(e.NewValue);
		}
		protected virtual void UpdateIconPlacement(ButtonIconPlacement buttonIconPlacement)
		{
		}
		protected int GetIconGridRow(ButtonIconPlacement buttonIconPlacement)
		{
			int result = 1;
			switch (buttonIconPlacement)
			{
			case ButtonIconPlacement.Top:
				result = 0;
				break;
			case ButtonIconPlacement.Bottom:
				result = 2;
				break;
			}
			return result;
		}
		protected int GetIconGridColumn(ButtonIconPlacement buttonIconPlacement)
		{
			int result = 1;
			switch (buttonIconPlacement)
			{
			case ButtonIconPlacement.Left:
				result = 0;
				break;
			case ButtonIconPlacement.Right:
				result = 2;
				break;
			}
			return result;
		}
		public void Dispose()
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}
		protected virtual void DisposeManagedResources()
		{
			HMIStatesButtonBaseViewModel wFStatesButtonBaseViewModel = base.DataContext as HMIStatesButtonBaseViewModel;
			if (wFStatesButtonBaseViewModel != null)
			{
				wFStatesButtonBaseViewModel.IconPlacementChanged -= new System.EventHandler<ValueChangedEventArgs<ButtonIconPlacement>>(this.viewModel_IconPlacementChanged);
			}
		}
		protected virtual void DisposeUnmanagedResources()
		{
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				if (disposing)
				{
					this.DisposeManagedResources();
				}
				this.DisposeUnmanagedResources();
			}
			this.isDisposed = true;
		}
	}
}
