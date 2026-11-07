// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Common.Browsers.ViewModels
{
	public interface ISelectorBrowserPageViewModel : IViewModel
	{
		event EventHandler<DialogClosedEventArgs> Closed;
		event EventHandler<ExceptionEventArgs> Error;
		string Title
		{
			get;
		}
		string SelectedValue
		{
			get;
			set;
		}
		bool CanRefresh
		{
			get;
		}
		bool IsVisible
		{
			get;
		}
		void Refresh(bool invalidateCache);
		void RaiseClosedEvent(bool dialogResult);
		T GetCachedValue<T>(string key, T defaultValue);
		void SetCachedValue<T>(string key, T value);
	}
	public interface ISelectorBrowserPageViewModel<TItemViewModel> : ISelectorBrowserPageViewModel
	{
		IObservableCollection<TItemViewModel> Values
		{
			get;
		}
		TItemViewModel SelectedItem
		{
			get;
			set;
		}
	}
}
