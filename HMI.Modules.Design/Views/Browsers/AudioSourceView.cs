// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using HMI.Common.Browsers.Views;
using HMI.Modules.Design.ViewModels.Browsers;
namespace HMI.Modules.Design.Views.Browsers
{
	[ExcludeFromCodeCoverage]
	public class AudioSourceView : BrowserView<IMediaBrowserPageViewModel<IAudioSourceViewModel>>
	{
		static AudioSourceView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(AudioSourceView), new FrameworkPropertyMetadata(typeof(AudioSourceView)));
		}
	}
}
