// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using HMI.Common.Browsers.Views;
using HMI.Modules.Design.ViewModels.Browsers;
namespace HMI.Modules.Design.Views.Browsers
{
	public class XamlSourceView : BrowserView<IMediaBrowserPageViewModel<IXamlSourceViewModel>>
	{
		static XamlSourceView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(XamlSourceView), new FrameworkPropertyMetadata(typeof(XamlSourceView)));
		}
	}
}
