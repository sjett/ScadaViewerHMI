// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Data;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public interface IServerSignalDefinitionBrowserViewModel : INotifyPropertyChanged, ISelectorBrowserPageViewModel<ISignalDefinitionViewModel>, IViewModel, ISelectorBrowserPageViewModel
	{
		IServersViewModel[] Servers
		{
			get;
		}
		bool ShowInactiveSignals
		{
			get;
			set;
		}
		//bool ShowLogs
		//{
		//	get;
		//	set;
		//}
		//bool ShowInactiveLogs
		//{
		//	get;
		//	set;
		//}
		CollectionViewSource FilteredValues
		{
			get;
		}
		IHierarchicalFilterViewModel SelectedFilter
		{
			get;
			set;
		}
		bool IsServerColumnVisible
		{
			get;
		}
		bool IsConnectorColumnVisible
		{
			get;
		}
		bool IsGroupColumnVisible
		{
			get;
		}
	}
}
