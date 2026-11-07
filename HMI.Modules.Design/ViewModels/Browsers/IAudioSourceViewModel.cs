// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IAudioSourceViewModel : IMediaSourceViewModel
	{
		Uri AudioSource
		{
			get;
		}
	}
}
