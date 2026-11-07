// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IXamlSourceViewModel : IMediaSourceViewModel, IBrowserItemViewModel
	{
		UIElement XamlSource
		{
			get;
		}
		bool IsValidXamlSource
		{
			get;
		}
	}
}
