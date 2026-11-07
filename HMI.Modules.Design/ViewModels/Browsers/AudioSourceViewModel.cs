// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Browsers.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	internal class AudioSourceViewModel : MediaSourceViewModel, IMediaSourceViewModel, IBrowserItemViewModel, IAudioSourceViewModel
	{
		public Uri? AudioSource
		{
			get
			{
				if(string.IsNullOrEmpty(base.FullPath))
                {
                    return null;
                }
                return new Uri(base.FullPath);
			}
		}
		public AudioSourceViewModel(string audioFileName) : base(audioFileName)
		{
		}
	}
}
