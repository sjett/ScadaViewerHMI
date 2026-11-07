// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Reactive;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Modules.Design.Resources;
using System.IO;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class AudioBrowserPageViewModel : MediaBrowserPageViewModel<IAudioSourceViewModel>
	{
		protected override string MediaFolder
		{
			get
			{
				return base.Solution.Project.ProjectMediaFolder;
			}
		}
		protected override MediaType MediaType
		{
			get
			{
				return MediaType.Audio;
			}
		}
		protected override string FileFilter
		{
			get
			{
				return EditorsTexts.BrowseAudioFilter;
			}
		}
		public override string Title
		{
			get
			{
				return BrowserTexts.AudioBrowserTitle;
			}
		}
		public AudioBrowserPageViewModel(IMediaService mediaService, IEventAggregator eventAggregator, ISolution solution, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(mediaService, eventAggregator, solution, uiScheduler, asyncScheduler)
		{
		}
		protected override IAudioSourceViewModel CreateViewModel(string fileName)
		{
			string audioFileName = null;
			if (fileName != null)
			{
				audioFileName = Path.Combine(this.MediaFolder, fileName);
			}
			return new AudioSourceViewModel(audioFileName);
		}
	}
}
