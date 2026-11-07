// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface ISelectorBrowserViewModel<T> : ISelectorBrowserViewModel, IBrowserViewModel, IDisposable, IViewModel, INotifyPropertyChanged where T : ISelectorBrowserPageViewModel
	{
	}
}
