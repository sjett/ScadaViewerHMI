// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IMediaSourceViewModel : IBrowserItemViewModel
	{
		string Name
		{
			get;
		}
		string FullPath
		{
			get;
		}
	}
}
