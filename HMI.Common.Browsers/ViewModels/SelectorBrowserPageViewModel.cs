// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using System.Collections.Specialized;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive.Concurrency;
using System.Runtime.Caching;

namespace HMI.Common.Browsers.ViewModels
{
	public abstract class SelectorBrowserPageViewModel<TItemModel, TItemViewModel> : DisposableViewModel, ISelectorBrowserPageViewModel<TItemViewModel>
		where TItemViewModel : class, IBrowserItemViewModel
	{
		protected const int BufferSize = 20;
		protected readonly IScheduler asyncScheduler;
		protected readonly IScheduler uiScheduler;
		private string _selectedValue;
		private TItemViewModel _selectedItem;

		public event EventHandler<DialogClosedEventArgs>? Closed;

		public event EventHandler<ExceptionEventArgs>? Error;

		public virtual bool CanRefresh
		{
			get
			{
				return true;
			}
		}
		public abstract string Title
		{
			get;
		}
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}
		public IObservableCollection<TItemViewModel> Values
		{
			get;
			protected set;
		}
		public string SelectedValue
		{
			get
			{
				return _selectedValue;
			}
			set
			{
				if (_selectedValue != value)
				{
                    _selectedValue = value;
					base.RaisePropertyChanged<string>(() => SelectedValue);
				}
			}
		}
		public TItemViewModel SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				if (!Equals(_selectedItem, value))
				{
                    _selectedItem = value;
					base.RaisePropertyChanged<TItemViewModel>(() => SelectedItem);
                    
                    if (value != null)
                    {
                        SelectedValue = value.ItemText;
                    }
                }
			}
		}
		public void RaiseClosedEvent(bool dialogResult)
		{
            Closed?.Invoke(this, new DialogClosedEventArgs(new bool?(dialogResult)));
		}
		public virtual void Refresh(bool invalidateCache)
		{
			if (!CanRefresh)
			{
				return;
			}
            Values.Clear();
            FillValues(invalidateCache);
		}
		protected SelectorBrowserPageViewModel(IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(uiScheduler != null, "uiScheduler");
			Contract.Requires<ArgumentNullException>(asyncScheduler != null, "asyncScheduler");

			this.uiScheduler = uiScheduler;
			this.asyncScheduler = asyncScheduler;

            Values = new FullyObservableCollection<TItemViewModel>();
            Values.CollectionChanged += new NotifyCollectionChangedEventHandler(OnValuesCollectionChanged);
		}

		protected abstract void FillValues(bool invalidateCache);

		protected abstract TItemViewModel CreateViewModel(TItemModel itemModel);

		protected void HandleRefreshError(Exception ex)
		{
            EndRefresh();
            OnError(ex);
		}
		protected virtual void OnError(Exception exception)
		{
            Error?.Invoke(this, new ExceptionEventArgs(exception));
		}
		protected virtual void EndRefresh()
		{
			TItemViewModel selectedItem = SelectedItem;
			if (selectedItem == null)
			{
				selectedItem = Values.FirstOrDefault<TItemViewModel>();
			}
			SelectedItem = selectedItem;
			base.RaisePropertyChanged<TItemViewModel>(() => SelectedItem);
			base.RaisePropertyChanged<string>(() => SelectedValue);
		}
		private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Reset || e.NewItems == null)
			{
				return;
			}
			IEnumerable<TItemViewModel> addedItems = e.NewItems.Cast<TItemViewModel>();
            SelectPreviouslyChosenItem(addedItems);
		}
		private void SelectPreviouslyChosenItem(IEnumerable<TItemViewModel> addedItems)
		{
			var matchingItem = addedItems.FirstOrDefault(item => item.ItemText == SelectedValue);
			if (matchingItem != null)
			{
                SelectedItem = matchingItem;
			}
		}

		public T GetCachedValue<T>(string key, T defaultValue)
		{
			string fullCacheKey = GetFullCacheKey(key);
			object obj = MemoryCache.Default[fullCacheKey] ?? defaultValue;
			return (T)obj;
		}

		public void SetCachedValue<T>(string key, T value)
		{
			MemoryCache.Default.Set(GetFullCacheKey(key), value, new CacheItemPolicy(), null);
		}

		protected abstract string GetFullCacheKey(string key);
		
	
	}
}
