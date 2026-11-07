// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Common.Browsers.ViewModels
{
	public class AlarmTypeTreeBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, IAlarmTypeTreeBrowserViewModel
	{
		public AlarmTypeTreeBrowserViewModel(IServerAlarmTypeTreeBrowserViewModel serverAlarmTypeTreeBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverAlarmTypeTreeBrowserViewModel != null, "serverAlarmTypeTreeBrowserViewModel");
			base.Pages.Add(serverAlarmTypeTreeBrowserViewModel);
		}
	}
}
