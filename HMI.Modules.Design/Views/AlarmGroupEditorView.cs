// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Browsers.Views;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class AlarmGroupEditorView : BrowserEditorView
	{
		protected override SelectorBrowserWindow CreateBrowserWindow()
		{
			IAlarmGroupBrowserViewModel instance = base.ServiceLocator.Resolve<IAlarmGroupBrowserViewModel>();
			return new AlarmGroupBrowser(instance);
		}
	}
}
