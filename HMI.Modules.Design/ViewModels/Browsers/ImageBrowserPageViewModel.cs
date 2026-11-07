// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Reactive;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Modules.Design.Resources;
using System.IO;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class ImageBrowserPageViewModel : MediaBrowserPageViewModel<IImageSourceViewModel>
	{
		protected override string MediaFolder
		{
			get
			{
				return base.Solution.Project.ProjectImagesFolder;
			}
		}
		protected override MediaType MediaType
		{
			get
			{
				return MediaType.Image;
			}
		}
		protected override string FileFilter
		{
			get
			{
				return EditorsTexts.BrowseImageFilter;
			}
		}
		public override string Title
		{
			get
			{
				return BrowserTexts.ImageBrowserTitle;
			}
		}
		public ImageBrowserPageViewModel(IMediaService mediaService, IEventAggregator eventAggregator, ISolution solution, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(mediaService, eventAggregator, solution, uiScheduler, asyncScheduler)
		{
		}
		protected override IImageSourceViewModel CreateViewModel(string fileName)
		{
			string imageFileName = null;
			if (fileName != null)
			{
				imageFileName = Path.Combine(this.MediaFolder, fileName);
			}
			return new ImageSourceViewModel(imageFileName);
		}
	}
}
