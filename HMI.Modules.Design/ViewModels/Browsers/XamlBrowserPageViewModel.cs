// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Reactive;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Resources;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	internal class XamlBrowserPageViewModel : MediaBrowserPageViewModel<IXamlSourceViewModel>
	{
		private readonly IFileSystemService _fileSystemService;
		protected override string MediaFolder
		{
			get
			{
				 return base.Solution.Project.ProjectXamlsFolder;
			}
		}
		protected override MediaType MediaType
		{
			get
			{
				return MediaType.Xaml;
			}
		}
		protected override string FileFilter
		{
			get
			{
				return EditorsTexts.BrowseXamlFilter;
			}
		}
		public override string Title
		{
			get
			{
				return BrowserTexts.XamlBrowserTitle;
			}
		}
		public XamlBrowserPageViewModel(IMediaService mediaService, IFileSystemService fileSystemService, IEventAggregator eventAggregator, ISolution solution, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(mediaService, eventAggregator, solution, uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(fileSystemService != null, "fileSystemService");
			this._fileSystemService = fileSystemService;
		}
		protected override IXamlSourceViewModel CreateViewModel(string fileName)
		{
			string xamlFileName = null;
			if (fileName != null)
			{
				xamlFileName = Path.Combine(this.MediaFolder, fileName);
			}
			return new XamlSourceViewModel(xamlFileName, this._fileSystemService);
		}
	}
}
