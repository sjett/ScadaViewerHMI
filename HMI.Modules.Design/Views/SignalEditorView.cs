// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using HMI.Common.Browsers.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class SignalEditorView : BrowserEditorView
	{
		protected override SelectorBrowserWindow CreateBrowserWindow()
		{
			return base.ServiceLocator.Resolve<SignalDefinitionBrowser>();
		}
	}
}
