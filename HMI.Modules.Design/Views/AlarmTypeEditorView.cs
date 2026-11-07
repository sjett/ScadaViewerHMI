// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Browsers.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class AlarmTypeEditorView : BrowserEditorView
	{
		protected override SelectorBrowserWindow CreateBrowserWindow()
		{
			IAlarmTypeBrowserViewModel instance = base.ServiceLocator.Resolve<IAlarmTypeBrowserViewModel>();
			return new AlarmTypeBrowser(instance);
		}
	}
}
