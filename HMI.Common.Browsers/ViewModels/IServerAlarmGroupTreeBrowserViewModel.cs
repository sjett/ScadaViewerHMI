// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Common.Browsers.ViewModels
{
	public interface IServerAlarmGroupTreeBrowserViewModel : INotifyPropertyChanged, ISelectorBrowserPageViewModel<ITranslatableViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerAlarmGroupBrowserViewModel
	{
		ICommand SelectAllCommand
		{
			get;
		}
		ICommand UnselectAllCommand
		{
			get;
		}
		ObservableCollection<AlarmTreeItem> AlarmGroupItems
		{
			get;
		}
	}
}
