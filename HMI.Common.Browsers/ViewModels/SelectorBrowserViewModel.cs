// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Reactive;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HMI.Common.Browsers.ViewModels
{
	public abstract class SelectorBrowserViewModel : BrowserViewModel, ISelectorBrowserViewModel
	{
		private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

		private ISelectorBrowserPageViewModel _activePage;
		private readonly DelegateCommand<bool?> _refreshCommand;
		private string _selectedValue;
		

		public IList<ISelectorBrowserPageViewModel> Pages
		{
			get;
			private set;
		}
		public bool AreMultiplePagesVisible
		{
			get
			{
				return this.Pages.Count(t=> t.IsVisible) > 1;
			}
		}
		public virtual ISelectorBrowserPageViewModel ActivePage
		{
			get
			{
				return this._activePage;
			}
			set
			{
				if (this._activePage == value)
				{
					return;
				}
				this._activePage = value;
				if (this._activePage != null && this.SelectedValue == null)
				{
					this.SelectedValue = this._selectedValue;
				}
				base.RaisePropertyChanged<ISelectorBrowserPageViewModel>(() => this.ActivePage);
				base.RaisePropertyChanged<string>(() => this.SelectedValue);
			}
		}
		public ICommand RefreshCommand
		{
			get
			{
				return this._refreshCommand;
			}
		}
		public string SelectedValue
		{
			get
			{
				if (this.ActivePage == null)
				{
					return this._selectedValue;
				}
				return this.ActivePage.SelectedValue;
			}
			set
			{
				this._selectedValue = value;
				if (this.ActivePage != null)
				{
					this.ActivePage.SelectedValue = value;
				}
			}
		}
		protected SelectorBrowserViewModel()
		{
			this._refreshCommand = new DelegateCommand<bool?>(
				this.ExecuteRefreshCommand, 
				this.CanExecuteRefreshCommand);

			this.Pages = new List<ISelectorBrowserPageViewModel>();

			SubscribeToSelectedValueChanges();
			SubscribeToPageErrors();
			SubscribeToPageClosedEvents();
			RefreshActivePageWhenReady();
		}
        void RefreshActivePageWhenReady()
        {
            var activePages = this.FromPropertyValues((SelectorBrowserViewModel x) => x.ActivePage)
                .Where(page => page != null && page.CanRefresh);

            compositeDisposable.Add(
				activePages
				.Do(page => page.Refresh(false))
				.Do(page => this.UpdateCommands())
				.Subscribe<ISelectorBrowserPageViewModel>());
        }
        void SubscribeToPageClosedEvents()
        {
            var pageClosedEvents = this.FromPropertyValues((SelectorBrowserViewModel x) => x.ActivePage)
                .Where(page => page != null)
                .SelectMany(ObservePageClosed);

            compositeDisposable.Add(
				pageClosedEvents
				.Select(closedEvent => closedEvent.EventArgs.DialogResult)
				.Do(this.OnCompleted)
				.Subscribe<bool?>());
        }
        void SubscribeToPageErrors()
        {
            var pageErrorEvents = this.FromPropertyValues((SelectorBrowserViewModel x) => x.ActivePage)
                .Where(page => page != null)
                .SelectMany(ObservePageErrors);

            compositeDisposable.Add(
				pageErrorEvents
				.Select(errorEvent => errorEvent.EventArgs.Exception)
				.Do(this.OnError)
				.Do(error => this.UpdateCommands())
				.Subscribe());
        }
        void SubscribeToSelectedValueChanges()
        {
            var activePages = this.FromPropertyValues((SelectorBrowserViewModel x) => x.ActivePage)
                .Where(page => page != null);

            compositeDisposable.Add(
				activePages
				.SelectMany(page => page.FromPropertyChanged((ISelectorBrowserPageViewModel x) => x.SelectedValue))
				.Do(change => base.RaisePropertyChanged<string>(() => this.SelectedValue))
				.Do(change => this.UpdateCommands())
				.Subscribe());
        }
        protected virtual void UpdateCommands()
		{
			this._refreshCommand.RaiseCanExecuteChanged();
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			this.compositeDisposable.Dispose();
		}
		protected virtual void ExecuteRefreshCommand(bool? ignoreCacheParam)
		{
			if (!this.CanExecuteRefreshCommand(ignoreCacheParam))
			{
				return;
			}
			this.ActivePage.Refresh(ignoreCacheParam.GetValueOrDefault());
		}
		protected virtual bool CanExecuteRefreshCommand(bool? ignoreCache)
		{
			return this.ActivePage != null && this.ActivePage.CanRefresh;
		}
		
		IObservable<EventPattern<DialogClosedEventArgs>> ObservePageClosed(ISelectorBrowserPageViewModel page)
		{
			return Observable
				.FromEventPattern<DialogClosedEventArgs>(
				addHandler => page.Closed += addHandler, 
				removeHandler => page.Closed -= removeHandler);
		}

		IObservable<EventPattern<ExceptionEventArgs>> ObservePageErrors(ISelectorBrowserPageViewModel page)
		{
			return Observable
				.FromEventPattern<ExceptionEventArgs>(
				addHandler => page.Error += addHandler, 
				removeHandler => page.Error -= removeHandler);
		}

	}
}
