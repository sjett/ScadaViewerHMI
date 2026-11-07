// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using HMI.Common.Browsers.Resources;
using HMI.Common.Browsers.Services;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
namespace HMI.Common.Browsers.ViewModels
{
	public class ServerSymbolicTextBrowserViewModel : SelectorBrowserPageViewModel<SymbolicText, ISymbolicTextViewModel>, INotifyPropertyChanged, ISelectorBrowserPageViewModel<ISymbolicTextViewModel>, IViewModel, ISelectorBrowserPageViewModel, IServerSymbolicTextBrowserViewModel
	{
	
		private readonly ISymbolicTextService symbolicTextService;
		private IDisposable symbolicTextSubscription;

		public event EventHandler? LanguagesChanging;

		public event EventHandler? LanguagesChanged;
        /// <summary>
        ///  阻止显示内部使用的名称（例如：信号定义、信号实例、服务器名称等）。如果启用此选项，则仅显示符号文本的名称。
        /// </summary>
        public bool SuppressInternallyUsedNames
		{
			get
			{
				return GetCachedValue("suppressInternallyUsedNames", true);
			}
			set
			{
				if (SuppressInternallyUsedNames == value)
				{
					return;
				}
                SetCachedValue("suppressInternallyUsedNames", value);
                RaisePropertyChanged(() => SuppressInternallyUsedNames);
                Refresh(false);
			}
		}
		public override string Title
		{
			get
			{
				return BrowserTexts.ServerSymbolicTexts;
			}
		}
		public IObservableCollection<ICultureViewModel> Languages
		{
			get;
			private set;
		}
		protected override void FillValues(bool invalidateCache)
		{
            ScheduleSymbolicTextRefresh(true, invalidateCache);

		}

		private void ScheduleSymbolicTextRefresh(bool loadLanguages, bool invalidateCache)
		{
			if (loadLanguages && Languages.Count == 0)
			{
				LoadLanguages(() => ScheduleSymbolicTextRefresh(false, invalidateCache));
				return;
			}
			asyncScheduler.Schedule(TimeSpan.FromSeconds(0.5), () => LoadSymbolicTexts(invalidateCache));
		}
		private void LoadSymbolicTexts(bool invalidateCache)
		{
			try
			{
				IncludedSymbolicTexts includedSymbolicTexts = !SuppressInternallyUsedNames ? IncludedSymbolicTexts.InternallyUsed : IncludedSymbolicTexts.SymbolicTexts;
				int[] languages = Languages.Select(language => language.ID).ToArray();

				IConnectableObservable<SymbolicText> symbolicTexts = symbolicTextService.GetSymbolicTexts(languages, includedSymbolicTexts, invalidateCache);
				var symbolicTextViewModels = symbolicTexts
					.Select(new Func<SymbolicText, ISymbolicTextViewModel>(CreateViewModel))
					.Buffer(20)
					.ObserveOn(uiScheduler);

				symbolicTextViewModels
					.SelectMany(t => t)
					.Subscribe(
					new Action<ISymbolicTextViewModel>(Values.Add),
					new Action<Exception>(HandleRefreshError),
					new Action(EndRefresh));

				symbolicTextSubscription = symbolicTexts.Connect();
			}
			catch (Exception ex) 
			{
                OnError(ex);
            }
		}
		protected override ISymbolicTextViewModel CreateViewModel(SymbolicText itemModel)
		{
			return new SymbolicTextViewModel(itemModel);
		}
		protected override string GetFullCacheKey(string key)
		{
			return "ServerSymbolicTextBrowserViewModel." + key;
		}
		private void LoadLanguages(Action onLoaded)
		{
			try
			{
				OnLanguagesChanging();
				IObservable<int> availableLanguageIds = symbolicTextService.GetAvailableLanguageIds();

				availableLanguageIds
					.Select(t => new CachedCultureViewModel(t))
					.ObserveOn(uiScheduler)
					.Subscribe(
						new Action<CachedCultureViewModel>(Languages.Add),
						new Action<Exception>(OnLanguageLoadError),
						new Action(() =>
						{
							this.LanguagesChanged?.Invoke(this, new EventArgs());
							onLoaded();
						}));
			}
			catch (Exception ex)
			{
				OnError(ex);
			}
		}
		void OnLanguagesChanging()
		{
			this.LanguagesChanging?.Invoke(this, new EventArgs());
		}
		private void OnLanguageLoadError(Exception exception)
		{
			OnLanguagesChanged();
			OnError(exception);
		}
		void OnLanguagesChanged()
		{
			this.LanguagesChanged?.Invoke(this, new EventArgs());
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			if (symbolicTextSubscription != null)
			{
				symbolicTextSubscription.Dispose();
			}
		}
		public ServerSymbolicTextBrowserViewModel(ISymbolicTextService symbolicTextService, IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(symbolicTextService != null, "symbolicTextService");
            this.symbolicTextService = symbolicTextService;
            Languages = new FullyObservableCollection<ICultureViewModel>();
		}
		
		
	}
}
