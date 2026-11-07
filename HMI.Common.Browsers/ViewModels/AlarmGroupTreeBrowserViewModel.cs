// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class AlarmGroupTreeBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, IAlarmGroupTreeBrowserViewModel
	{
		public AlarmGroupTreeBrowserViewModel(IServerAlarmGroupTreeBrowserViewModel serverAlarmGroupTreeBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverAlarmGroupTreeBrowserViewModel != null, "serverAlarmGroupTreeBrowserViewModel");
			base.Pages.Add(serverAlarmGroupTreeBrowserViewModel);
		}
	}
}
