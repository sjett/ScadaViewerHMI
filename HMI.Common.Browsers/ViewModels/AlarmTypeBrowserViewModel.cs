// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class AlarmTypeBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, IAlarmTypeBrowserViewModel
	{
		public AlarmTypeBrowserViewModel(IServerAlarmTypeBrowserViewModel serverAlarmTypeBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverAlarmTypeBrowserViewModel != null, "serverAlarmTypeBrowserViewModel");

			base.Pages.Add(serverAlarmTypeBrowserViewModel);
		}
	}
}
