// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using HMI.Common.Browsers.Resources;
using HMI.Common.Browsers.Services;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace HMI.Common.Browsers.ViewModels
{
	public class ServerProjectAuthorizationBrowserViewModel : SelectorBrowserPageViewModel<ProjectAuthorization, IProjectAuthorizationViewModel>, INotifyPropertyChanged, ISelectorBrowserPageViewModel<IProjectAuthorizationViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerProjectAuthorizationBrowserViewModel
	{
		private readonly IProjectAuthorizationService projectAuthorizationService;
	
		public override string Title
		{
			get
			{
				return BrowserTexts.ServerProjectAuthorizations;
			}
		}
		protected override void FillValues(bool invalidateCache)
		{
            asyncScheduler.Schedule(TimeSpan.FromSeconds(0.5), LoadProjectAuthorizations);
		}
		private void LoadProjectAuthorizations()
		{
			try
			{
				IObservable<IList<IProjectAuthorizationViewModel>> authorizationBatches = projectAuthorizationService
					.GetProjectAuthorizations()
					.Select(new Func<ProjectAuthorization, IProjectAuthorizationViewModel>(CreateViewModel))
					.Buffer(BufferSize)
					.ObserveOn(uiScheduler);

				authorizationBatches.SelectMany(batch => batch)
					.Subscribe(
					new Action<IProjectAuthorizationViewModel>(Values.Add),
					new Action<Exception>(HandleRefreshError),
					new Action(EndRefresh));
			}
			catch (Exception ex)
			{
				OnError(ex);
			}
		}
		protected override IProjectAuthorizationViewModel CreateViewModel(ProjectAuthorization itemModel)
		{
			return new ProjectAuthorizationViewModel(itemModel);
		}
		protected override string GetFullCacheKey(string key)
		{
			return "ServerProjectAuthorizationBrowserViewModel." + key;
		}
		public ServerProjectAuthorizationBrowserViewModel(IProjectAuthorizationService projectAuthorizationService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(projectAuthorizationService != null, "alarmTypeService");
            this.projectAuthorizationService = projectAuthorizationService;
		}

    }
}
