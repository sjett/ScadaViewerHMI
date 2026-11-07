// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Media;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IImageSourceViewModel : IMediaSourceViewModel, IBrowserItemViewModel
	{
		ImageSource BitmapSource
		{
			get;
		}
	}
}
