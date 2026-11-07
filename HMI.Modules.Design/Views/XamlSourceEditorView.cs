// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using HMI.Common.Browsers.Views;
using HMI.Modules.Design.Views.Browsers;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class XamlSourceEditorView : MediaSourceEditorView
	{
		protected override SelectorBrowserWindow CreateBrowserWindow()
		{
			return base.ServiceLocator.Resolve<XamlBrowser>();
		}
	}
}
