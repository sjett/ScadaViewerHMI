// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class AlarmGroupBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, IAlarmGroupBrowserViewModel
	{
		public AlarmGroupBrowserViewModel(IServerAlarmGroupBrowserViewModel serverAlarmGroupBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverAlarmGroupBrowserViewModel != null, "serverAlarmGroupBrowserViewModel");
			base.Pages.Add(serverAlarmGroupBrowserViewModel);
		}
	}
}
