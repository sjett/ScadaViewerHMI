// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Events.Interactions;
using HMI.Common.Presentation.Reactive;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Modules.Design.Resources;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public abstract class MediaBrowserPageViewModel<TItemViewModel> : SelectorBrowserPageViewModel<string, TItemViewModel>, IMediaBrowserPageViewModel<TItemViewModel>
		where TItemViewModel : class, IMediaSourceViewModel
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IMediaService _mediaService;
		
		protected ISolution Solution
		{
			get;
			private set;
		}
		protected abstract string MediaFolder
		{
			get;
		}
		protected abstract MediaType MediaType
		{
			get;
		}
		protected abstract string FileFilter
		{
			get;
		}
		public ICommand BrowseCommand
		{
			get;
			private set;
		}
		protected MediaBrowserPageViewModel(IMediaService mediaService, IEventAggregator eventAggregator, ISolution solution, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(solution != null, "solution");
			Contract.Requires<ArgumentNullException>(mediaService != null, "mediaService");
			Contract.Requires<ArgumentNullException>(eventAggregator != null, "eventAggregator");

			this.Solution = solution;
			this._mediaService = mediaService;
			this._eventAggregator = eventAggregator;
			this.BrowseCommand = new DelegateCommand(new Action(this.OnBrowseCommand));
		}
		private void OnBrowseCommand()
		{
			OpenFileInteractionDetails details = new OpenFileInteractionDetails(this.FileFilter, new OpenFileInteractionCallback(this.OnFilesSelected), this.MediaType)
			{
				Multiselect = true
			};
			this._eventAggregator.PublishOpenFileInteractionEvent(details);
		}
		private void OnFilesSelected(bool wasAccepted, IEnumerable<string> selectedFilePaths, object mediaType)
		{
			if (!wasAccepted)
			{
				return;
			}
			this.AddSelectedMediaFiles(selectedFilePaths, (MediaType)mediaType);
			this.Refresh(true);
		}
		private void AddSelectedMediaFiles(IEnumerable<string> filePaths, MediaType mediaType)
		{
			List<Exception> errors = new List<Exception>();
			foreach (string filePath in filePaths)
			{
				try
				{
					this._mediaService.AddMedia(filePath, this.MediaFolder, mediaType);
				}
				catch (Exception exception)
				{
					errors.Add(exception);
				}
			}
			if (errors.Count > 0)
			{
				PublishMediaImportWarning(errors);
			}
		}
		protected override string GetFullCacheKey(string key)
		{
			return "MediaBrowserPageViewModel." + key;
		}
		private void PublishMediaImportWarning(IEnumerable<Exception> errors)
		{
			AggregateException exception = new AggregateException(errors);
			this._eventAggregator.PublishNotification(NotificationSeverity.Warning, ErrorTexts.ErrorAddingFilesToProject, exception.Message, exception);
		}
		protected override void FillValues(bool invalidateCache)
		{
            IObservable<string> first = System.Reactive.Linq.Observable.Repeat<string>((string)null, 1);
			IObservable<string> source = first.Concat(this._mediaService.GetMediaFiles(this.MediaType, this.MediaFolder));

			source
				.Select(new Func<string, TItemViewModel>(this.CreateViewModel))
				.ObserveOn(this.uiScheduler)
				.Subscribe(
				new Action<TItemViewModel>(base.Values.Add), 
				new Action<Exception>(base.HandleRefreshError),
				new Action(this.EndRefresh));
		}
	}
}
