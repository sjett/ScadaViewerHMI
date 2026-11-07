// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Collections;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public interface IServerSymbolicTextBrowserViewModel : INotifyPropertyChanged, ISelectorBrowserPageViewModel<ISymbolicTextViewModel>, IViewModel, ISelectorBrowserPageViewModel
	{
		event EventHandler LanguagesChanging;
		event EventHandler LanguagesChanged;
		IObservableCollection<ICultureViewModel> Languages
		{
			get;
		}
		bool SuppressInternallyUsedNames
		{
			get;
			set;
		}
	}
}
