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
using System.Reactive.Subjects;
using System.Windows.Data;

namespace HMI.Common.Browsers.ViewModels
{
	public class ServerSignalDefinitionBrowserViewModel : SelectorBrowserPageViewModel<SignalDefinition, ISignalDefinitionViewModel>, INotifyPropertyChanged, ISelectorBrowserPageViewModel<ISignalDefinitionViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerSignalDefinitionBrowserViewModel
	{
		private const int AllSignalsLanguageId = -1;
		private readonly ISignalsService signalsService;
        private IHierarchicalFilterViewModel filterBeforeRefresh;
		private IDisposable signalDefinitionsSubscription;

		public IServersViewModel[] Servers
		{
			get;
			private set;
		}
		public CollectionViewSource FilteredValues
		{
			get;
			private set;
		}
		public bool IsServerColumnVisible
		{
			get
			{
				return SelectedFilter == null || SelectedFilter is ServersViewModel;
			}
		}
		public bool IsConnectorColumnVisible
		{
			get
			{
				return IsServerColumnVisible || SelectedFilter is ServerViewModel;
			}
		}
		public bool IsGroupColumnVisible
		{
			get
			{
				return IsConnectorColumnVisible || SelectedFilter is ConnectorViewModel;
			}
		}
		public override string Title
		{
			get
			{
				return BrowserTexts.ServerSignalDefinitions;
			}
		}
		public virtual bool ShowInactiveSignals
		{
			get
			{
				return GetCachedValue("showInactiveSignals", false);
			}
			set
			{
				if (ShowInactiveSignals == value)
				{
					return;
				}
                RaisePropertyChanging(() => ShowInactiveSignals);
                SetCachedValue("showInactiveSignals", value);
                RaisePropertyChanged(() => ShowInactiveSignals);
                RefreshFilteredValues();
			}
		}
		public virtual IHierarchicalFilterViewModel SelectedFilter
		{
			get
			{
				return GetCachedValue<IHierarchicalFilterViewModel>("selectedFilter", null);
			}
			set
			{
				if (SelectedFilter == value)
				{
					return;
				}
                RaisePropertyChanging(() => SelectedFilter);
                SetCachedValue("selectedFilter", value);
                RaisePropertyChanged(() => SelectedFilter);
                RefreshFilteredValues();
                RaisePropertyChanged(() => IsServerColumnVisible);
                RaisePropertyChanged(() => IsConnectorColumnVisible);
                RaisePropertyChanged(() => IsGroupColumnVisible);
			}
		}
		protected override string GetFullCacheKey(string key)
		{
			return "ServerSignalDefinitionBrowserViewModel." + key;
		}
		protected override void FillValues(bool invalidateCache)
		{
            asyncScheduler.Schedule(TimeSpan.FromSeconds(0.5), () => LoadSignalDefinitions(invalidateCache));
		}
		private void LoadSignalDefinitions(bool invalidateCache)
		{
			try
			{
				IConnectableObservable<SignalDefinition> signalDefinitions = signalsService.GetSignalDefinitions(AllSignalsLanguageId, invalidateCache);
				filterBeforeRefresh = SelectedFilter;
				IObservable<IList<ISignalDefinitionViewModel>> signalDefinitionBatches = signalDefinitions
					.Select(new Func<SignalDefinition, ISignalDefinitionViewModel>(CreateViewModel)).Buffer(20).ObserveOn(uiScheduler);

				signalDefinitionBatches
					.SelectMany(t => t)
					.Do(AddSignalToFilterTree)
					.Subscribe(
					new Action<ISignalDefinitionViewModel>(Values.Add),
					new Action<Exception>(HandleRefreshError),
					new Action(EndRefresh));

				signalDefinitionsSubscription = signalDefinitions.Connect();
			}
			catch (Exception ex)
			{
				OnError(ex);
			}
		}
		protected override void EndRefresh()
		{
			base.EndRefresh();
			RestoreFilterAfterRefresh();
		}
        void RestoreFilterAfterRefresh()
        {
            this.SelectedFilter = this.filterBeforeRefresh;
        }
        private void AddSignalToFilterTree(ISignalDefinitionViewModel signal)
		{
            Servers[0].AddServer(signal.Server, signal.ServerDescription ?? signal.Server).AddConnector(signal.Connector, signal.ConnectorDescription ?? signal.Connector).AddSignalGroup(signal.Group, signal.GroupDescription ?? signal.Group);
		}
		protected override ISignalDefinitionViewModel CreateViewModel(SignalDefinition itemModel)
		{
			return new SignalDefinitionViewModel(
				//itemModel.Log,
				itemModel);
		}
		private void FilterSignalDefinition(object sender, FilterEventArgs e)
		{
			ISignalDefinitionViewModel signalDefinitionViewModel = (ISignalDefinitionViewModel)e.Item;
			e.Accepted = true;
			if (signalDefinitionViewModel == null)
			{
				e.Accepted = false;
				return;
			}
			if (!ShowInactiveSignals && !signalDefinitionViewModel.IsActive)
			{
				e.Accepted = false;
				return;
			}
			//if (ShowLogs && !signalDefinitionViewModel.LogId.HasValue)
			//{
			//	e.Accepted = false;
			//	return;
			//}
			//if (!ShowLogs && signalDefinitionViewModel.LogId.HasValue)
			//{
			//	e.Accepted = false;
			//	return;
			//}
			//if (ShowLogs && !ShowInactiveLogs && !signalDefinitionViewModel.IsLogActive)
			//{
			//	e.Accepted = false;
			//	return;
			//}
			if (SelectedFilter != null && !SelectedFilter.IsMatchingFilter(signalDefinitionViewModel))
			{
				e.Accepted = false;
			}
		}
		private void RefreshFilteredValues()
		{
			if (FilteredValues != null && FilteredValues.View != null)
			{
                FilteredValues.View.Refresh();
			}
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			if (signalDefinitionsSubscription != null)
			{
                signalDefinitionsSubscription.Dispose();
			}
		}
		public ServerSignalDefinitionBrowserViewModel(ISignalsService signalsService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(signalsService != null, "signalsService");
            this.signalsService = signalsService;
            Servers = new IServersViewModel[]
			{
				new ServersViewModel()
			};
            FilteredValues = new CollectionViewSource();
            FilteredValues.Filter += new FilterEventHandler(FilterSignalDefinition);
            FilteredValues.Source = Values;
            RaisePropertyChanged(() => FilteredValues);
		}
		
	}
}
