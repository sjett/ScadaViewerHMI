// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using System.Windows.Input;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IMediaBrowserPageViewModel<TItemViewModel> : ISelectorBrowserPageViewModel<TItemViewModel>, IViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel
	{
		ICommand BrowseCommand
		{
			get;
		}
	}
}
