// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using HMI.Common.Browsers.Models;
using HMI.Common.Browsers.Resources;
using HMI.Common.Browsers.Services;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Common.Browsers.ViewModels
{
	public class ServerAlarmGroupBrowserViewModel : SelectorBrowserPageViewModel<TranslatedEntity, ITranslatableViewModel>, INotifyPropertyChanged, ISelectorBrowserPageViewModel<ITranslatableViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerAlarmGroupBrowserViewModel
	{
		private readonly IAlarmService alarmService;
		private IDisposable alarmGroupsSubscription;
		public override string Title
		{
			get
			{
				return BrowserTexts.ServerAlarmGroups;
			}
		}
		protected override void FillValues(bool invalidateCache)
		{
            asyncScheduler.Schedule(TimeSpan.FromSeconds(0.5), LoadAlarmGroups);
		}
		private void LoadAlarmGroups()
		{
			IObservable<IList<ITranslatableViewModel>> alarmGroupBatches = alarmService.GetAlarmGroups()
                .Select(new Func<TranslatedEntity, ITranslatableViewModel>(CreateViewModel))
                .Buffer(BufferSize)
                .ObserveOn(uiScheduler);
            alarmGroupsSubscription = alarmGroupBatches
                .SelectMany(batch => batch)
                .Subscribe(new Action<ITranslatableViewModel>(Values.Add), new Action<Exception>(HandleRefreshError), new Action(EndRefresh));
		}
		protected override ITranslatableViewModel CreateViewModel(TranslatedEntity itemModel)
		{
			return new TranslatableViewModel(itemModel);
		}
		protected override string GetFullCacheKey(string key)
		{
			return "ServerAlarmGroupBrowserViewModel." + key;
		}
		public ServerAlarmGroupBrowserViewModel(IAlarmService alarmService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(alarmService != null, "alarmService");
            this.alarmService = alarmService;
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			if (alarmGroupsSubscription != null)
			{
                alarmGroupsSubscription.Dispose();
			}
		}
	}
}
