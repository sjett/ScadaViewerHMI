// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.ViewModels;
using System.Collections.Specialized;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive;
using System.Reactive.Linq;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class HorizontalCollectionBrowserViewModel : CollectionBrowserViewModel, IHorizontalCollectionBrowserViewModel
	{
		private bool _isSelectedItemSyncLocked;
		private readonly IDisposable _selectedItemChangedSubscription;
		
		public IHorizontalPropertyEditorViewModel HorizontalPropertyEditorViewModel
		{
			get;
			private set;
		}
		public override bool ExpandAllGroups
		{
			get
			{
				return base.ExpandAllGroups;
			}
			set
			{
				base.ExpandAllGroups = value;
				this.HorizontalPropertyEditorViewModel.ExpandAllGroups = value;
			}
		}
		public override string GroupBy
		{
			get
			{
				return base.GroupBy;
			}
			set
			{
				base.GroupBy = value;
				this.HorizontalPropertyEditorViewModel.GroupBy = value;
			}
		}
		public override bool IsGroupingEnabled
		{
			get
			{
				return base.IsGroupingEnabled;
			}
			set
			{
				base.IsGroupingEnabled = value;
				this.HorizontalPropertyEditorViewModel.IsCollectionGroupingEnabled = value;
			}
		}
		public override ICollectionEditorItem SelectedItem
		{
			get
			{
				return base.SelectedItem;
			}
			set
			{
				base.SelectedItem = value;

				if (!this._isSelectedItemSyncLocked)
				{
					this.HorizontalPropertyEditorViewModel.SelectedItem =
						this.HorizontalPropertyEditorViewModel.Items.FirstOrDefault(item => item.Value == value.Item);
				}
			}
		}
		public HorizontalCollectionBrowserViewModel(IServiceProvider services, IActionMonitor actionMonitor, IPropertyInspectorService propertyInspectorService, IHorizontalPropertyEditorViewModel horizontalPropertyEditorViewModel) : base(services, actionMonitor, propertyInspectorService)
		{
			Contract.Requires<ArgumentNullException>(horizontalPropertyEditorViewModel != null, "horizontalPropertyEditorViewModel");

			this.HorizontalPropertyEditorViewModel = horizontalPropertyEditorViewModel;

			var selectedItemChanges = Observable
							.FromEventPattern<ValueChangedEventArgs<HorizontalEditorItem>>(
								addHandler => this.HorizontalPropertyEditorViewModel.SelectedItemChanged += addHandler, 
								removeHandler => this.HorizontalPropertyEditorViewModel.SelectedItemChanged -= removeHandler);

            this._selectedItemChangedSubscription = selectedItemChanges
							.Do(_ => this._isSelectedItemSyncLocked = true)
							.Do(change => this.SelectedItem = FindCollectionEditorItem(change.EventArgs.NewValue))
							.Do(_ => this._isSelectedItemSyncLocked = false)
							.Subscribe<EventPattern<ValueChangedEventArgs<HorizontalEditorItem>>>();

			base.Items.CollectionChanged += OnCollectionItemsChanged;
		}
		private ICollectionEditorItem FindCollectionEditorItem(HorizontalEditorItem editorItem)
		{
			if (editorItem == null)
			{
				return null;
			}
			var collectionItem = editorItem.Value as ICollectionItem;
			if (collectionItem != null)
			{
				return base.Items.SingleOrDefault(item => item.Item == collectionItem);
			}
			return null;
		}
		private void OnCollectionItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					using (IEnumerator<ICollectionEditorItem> enumerator = e.NewItems.OfType<ICollectionEditorItem>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ICollectionEditorItem addedItem = enumerator.Current;
							this.HorizontalPropertyEditorViewModel.AddObjectToItems(addedItem.Item);
						}
						return;
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					break;
				case NotifyCollectionChangedAction.Replace:
					return;
				case NotifyCollectionChangedAction.Move:
					this.HorizontalPropertyEditorViewModel.Items.Move(e.OldStartingIndex, e.NewStartingIndex);
					return;
                case NotifyCollectionChangedAction.Reset:
					this.HorizontalPropertyEditorViewModel.SetDisplayObjects(base.GetParentCollection());
					return;
				default:
					return;
			}

			using (IEnumerator<ICollectionEditorItem> removedItemsEnumerator = e.OldItems.OfType<ICollectionEditorItem>().GetEnumerator())
			{
				while (removedItemsEnumerator.MoveNext())
				{
					ICollectionEditorItem removedItem = removedItemsEnumerator.Current;

					var item = this.HorizontalPropertyEditorViewModel.Items.SingleOrDefault(editorItem => editorItem.Value == removedItem.Item);

					this.HorizontalPropertyEditorViewModel.Items.Remove(item);
				}
				return;
			}
		}
		public override void Initialize(IBindingSource bindingSource)
		{
			base.Initialize(bindingSource);
			this.HorizontalPropertyEditorViewModel.SetDisplayObjects(base.GetParentCollection());
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			this._selectedItemChangedSubscription.Dispose();
		}
		
		
	}
}
