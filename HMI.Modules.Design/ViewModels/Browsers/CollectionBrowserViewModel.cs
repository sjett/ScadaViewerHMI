// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Instrumentation.Metadata;
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes.Validation;
using System.Collections;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;

namespace HMI.Modules.Design.ViewModels.Browsers
{
    public class CollectionEditorItem : PropertyChangedNotifier, ICollectionEditorItem
    {
        private string _displayText;

        public event EventHandler<ValueChangedEventArgs<string>>? DisplayTextChanged;

        public string Key
        {
            get;
            set;
        }
        public ICollectionItem Item
        {
            get;
            set;
        }
        public string DisplayText
        {
            get
            {
                return this._displayText;
            }
            set
            {
                if (this._displayText != value)
                {
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this._displayText, value);
                    this._displayText = value;
                    this.DisplayTextChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.DisplayText);
                }
            }
        }
        public CollectionEditorItem(string key, ICollectionItem item)
        {
            this.DisplayText = item.ToString();
            this.Key = key;
            this.Item = item;
            item.EditorDisplayTextChanged += new EventHandler<EventArgs>(this.OnItemDisplayTextChanged);
        }
        private void OnItemDisplayTextChanged(object sender, EventArgs e)
        {
            this.DisplayText = this.Item.ToString();
        }
    }

    public abstract class CollectionBrowserViewModel : BrowserViewModel, ICollectionBrowserViewModel
	{

        private readonly IActionMonitor _actionMonitor;
		protected readonly IPropertyInspectorService propertyInspectorService;
        private readonly IServiceProvider _serviceProvider;
		private bool _isCollectionReadOnly;
		private Type _defaultItemType;
		private readonly IObservableCollection<ICollectionEditorItem> items;
		private ICollectionEditorItem selectedItem;

        private DelegateCommand _deleteItemCommand;
        private DelegateCommand _moveItemUpCommand;
        private DelegateCommand _moveItemDownCommand;
        private DelegateCommand<Type> _addNewItemCommand;
        private DelegateCommand _duplicateItemCommand;

		private bool _isGroupingEnabled;
		private bool _expandAllGroups;
		private string _groupBy;
		private Type[] _availableItemTypes;
		
		
		public virtual bool IsCollectionReadOnly
		{
			get
			{
				return this._isCollectionReadOnly;
			}
			set
			{
				if (this._isCollectionReadOnly == value)
				{
					return;
				}
				base.RaisePropertyChanging<bool>(() => this.IsCollectionReadOnly);
				this._isCollectionReadOnly = value;
				base.RaisePropertyChanged<bool>(() => this.IsCollectionReadOnly);
				base.RaisePropertyChanged<bool>(() => this.IsCollectionEditable);
			}
		}
		public bool IsCollectionEditable
		{
			get
			{
				return !this.IsCollectionReadOnly;
			}
		}
		public Type DefaultItemType
		{
			get
			{
				return this._defaultItemType;
			}
			set
			{
				if (this._defaultItemType == value)
				{
					return;
				}
				this._defaultItemType = value;
				base.RaisePropertyChanged<Type>(() => this.DefaultItemType);
			}
		}
		public IObservableCollection<ICollectionEditorItem> Items
		{
			get
			{
				return this.items;
			}
		}
		public virtual ICollectionEditorItem SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				if (this.selectedItem == value)
				{
					return;
				}
				this.selectedItem = value;
				base.RaisePropertyChanged<ICollectionEditorItem>(() => this.SelectedItem);
				this.UpdateCommandStates();
			}
		}
		public ICommand DeleteItemCommand
		{
			get
			{
				return this._deleteItemCommand;
			}
		}
		public ICommand MoveItemUpCommand
		{
			get
			{
				return this._moveItemUpCommand;
			}
		}
		public ICommand MoveItemDownCommand
		{
			get
			{
				return this._moveItemDownCommand;
			}
		}
		public ICommand AddNewItemCommand
		{
			get
			{
				return this._addNewItemCommand;
			}
		}
		public ICommand DuplicateItemCommand
		{
			get
			{
				return this._duplicateItemCommand;
			}
		}
		public virtual bool IsGroupingEnabled
		{
			get
			{
				return this._isGroupingEnabled;
			}
			set
			{
				if (this._isGroupingEnabled == value)
				{
					return;
				}
				this._isGroupingEnabled = value;
				base.RaisePropertyChanged<bool>(() => this.IsGroupingEnabled);
			}
		}
		public virtual bool ExpandAllGroups
		{
			get
			{
				return this._expandAllGroups;
			}
			set
			{
				if (this._expandAllGroups == value)
				{
					return;
				}
				this._expandAllGroups = value;
				base.RaisePropertyChanged<bool>(() => this.ExpandAllGroups);
			}
		}
		public virtual string GroupBy
		{
			get
			{
				return this._groupBy;
			}
			set
			{
				if (this._groupBy == value)
				{
					return;
				}
				this._groupBy = value;
				base.RaisePropertyChanged<string>(() => this.GroupBy);
			}
		}
		public bool AllowDrop
		{
			get
			{
				return this.AvailableItemTypes.Length > 0;
			}
		}
		public Type[] AvailableItemTypes
		{
			get
			{
				return this._availableItemTypes ?? new Type[0];
			}
			set
			{
				this._availableItemTypes = value;
			}
		}
		public bool CanMoveItems
		{
			get;
			private set;
		}
		public IBindingSource BindingSource
		{
			get;
			private set;
		}
		protected CollectionBrowserViewModel(IServiceProvider services, IActionMonitor actionMonitor, IPropertyInspectorService propertyInspectorService) : base()
		{
			this.items = new FullyObservableCollection<ICollectionEditorItem>();

			Contract.Requires<ArgumentNullException>(services != null, "services");
			Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");
			Contract.Requires<ArgumentNullException>(propertyInspectorService != null, "propertyInspectorService");

			this._serviceProvider = services;
			this._actionMonitor = actionMonitor;
			this.propertyInspectorService = propertyInspectorService;

			var selectedItemChanges = this.FromPropertyValues((CollectionBrowserViewModel x) => x.SelectedItem);

			var selectedItemsWithValues = selectedItemChanges.Where(t=> t != null && t.Item != null);

			IObservable<ICollectionItem> selectedCollectionItems = selectedItemsWithValues.Select(t=> t.Item);

			IObservable<EventPattern<PropertyChangedEventArgs>> selectedItemPropertyChanges = selectedCollectionItems.SelectMany(ObserveItemPropertyChanges);

			selectedItemPropertyChanges
				.Do(this.OnItemPropertyChanged)
				.Subscribe<EventPattern<PropertyChangedEventArgs>>();

			InitializeCommands();
		}
        private void InitializeCommands()
        {
            this._deleteItemCommand = new DelegateCommand(new Action(this.DeleteSelectedItem), new Func<bool>(this.CanDeleteSelectedItem));
            this._moveItemUpCommand = new DelegateCommand(new Action(this.MoveSelectedItemUp), new Func<bool>(this	.CanMoveSelectedItemUp));
            this._moveItemDownCommand = new DelegateCommand(new Action(this.MoveSelectedItemDown), new Func<bool>(this.CanMoveSelectedItemDown));
            this._addNewItemCommand = new DelegateCommand<Type>(new Action<Type>(this.AddNewItem));
            this._duplicateItemCommand = new DelegateCommand(new Action(this.DuplicateSelectedItem), new Func<bool>(this.CanDuplicateSelectedItem));
        }

        void DeleteSelectedItem()
		{
			if (!this.CanDeleteSelectedItem())
			{
				return;
			}
			this.Items.Remove(this.SelectedItem);
			this.SelectedItem = this.Items.FirstOrDefault<ICollectionEditorItem>();
			this.UpdateCommandStates();
		}
		bool CanDeleteSelectedItem()
		{
			return this.SelectedItem != null;
		}
		void MoveSelectedItemUp()
		{
			if (!this.CanMoveSelectedItemUp())
			{
				return;
			}
			this.MoveItem(this.SelectedItem, -1);
			this.UpdateCommandStates();
		}
		bool CanMoveSelectedItemUp()
		{
			return this.SelectedItem != null && this.SelectedItem != this.Items.FirstOrDefault<ICollectionEditorItem>();
		}
		void MoveSelectedItemDown()	
		{
			if (!this.CanMoveSelectedItemDown())
			{
				return;
			}
			this.MoveItem(this.SelectedItem, 1);
			this.UpdateCommandStates();
		}
		bool CanMoveSelectedItemDown()
		{
			return this.SelectedItem != null && this.SelectedItem != this.Items.LastOrDefault<ICollectionEditorItem>();
		}
		private void AddNewItem(Type itemType)
		{
			ICollectionEditorItem selectedItem = this.CreateAndAddItem(itemType);
			this.SelectedItem = selectedItem;
			this.UpdateDefaultItemTypeAfterAdd(itemType);
		}
		bool CanDuplicateSelectedItem()
		{
			return this.SelectedItem != null;
		}
		void DuplicateSelectedItem()
		{
			if (!this.CanDuplicateSelectedItem())
			{
				return;
			}
			ICollectionEditorItem collectionEditorItem = this.CloneAndAddItem(this.SelectedItem.Item);
			collectionEditorItem.Item.Id = Guid.NewGuid();
			this.SelectedItem = collectionEditorItem;
		}
		public void ApplyChanges()
		{
			IList originalCollection = GetOriginalCollection();
			this.Items.UpdateOriginalCollection(originalCollection);
			this._actionMonitor.EndGrouping();
		}
        private IList GetOriginalCollection()
        {
            return (IList)Reflection.GetPropertyValue(BindingSource.SourceInstance, BindingSource.PropertyPath);
        }
        public void CancelChanges()
		{
			this._actionMonitor.EndGrouping();
			this._actionMonitor.CancelAction();
		}
		public virtual void Initialize(IBindingSource bindingSource)
		{
			Contract.Requires<ArgumentNullException>(bindingSource != null, "bindingSource");
			this.BindingSource = bindingSource;
			this._actionMonitor.StartGrouping();

			IList list = GetOriginalCollection();
			DetectCollectionMoveSupport(list);
			WrapExistingItems(list.OfType<ICollectionItem>());
			this.UpdateCommandStates();
		}
        private void DetectCollectionMoveSupport(IList collection)
        {
            var moveMethod = collection.GetType().GetMethod("Move");
            if (moveMethod == null)
            {
                CanMoveItems = false;
                return;
            }
            ParameterInfo[] parameters = moveMethod.GetParameters();
            CanMoveItems = (parameters.Length == 2 && parameters[0].ParameterType == typeof(int) && parameters[1].ParameterType == typeof(int));
        }
        private void WrapExistingItems(IEnumerable<ICollectionItem> collectionItems)
        {
            foreach (ICollectionItem collectionItem in collectionItems)
            {
                CloneAndAddItem(collectionItem);
            }
        }
        private void UpdateCommandStates()
		{
			this._moveItemUpCommand.RaiseCanExecuteChanged();
			this._moveItemDownCommand.RaiseCanExecuteChanged();
			this._deleteItemCommand.RaiseCanExecuteChanged();
			this._duplicateItemCommand.RaiseCanExecuteChanged();

			DelegateCommandBase? delegateCommandBase = base.OKCommand as DelegateCommandBase;
			if (delegateCommandBase != null)
			{
				delegateCommandBase.RaiseCanExecuteChanged();
			}
		}
		private ICollectionEditorItem CreateAndAddItem(Type itemType)
		{
			ICollectionItem collectionItem = CreateCollectionItem(itemType);
			collectionItem.ParentCollection = this.GetParentCollection();
			ICollectionEditorItem collectionEditorItem = CreateEditorItem(collectionItem);
			this.Items.Add(collectionEditorItem);
			return collectionEditorItem;
		}
        private ICollectionItem CreateCollectionItem(Type itemType)
        {
            return Activator.CreateInstance(itemType,
            [
                _serviceProvider,
                BindingSource.SourceInstance
            ]) as ICollectionItem;
        }
        private ICollectionEditorItem CreateEditorItem(ICollectionItem collectionItem)
        {
            string key = GetGroupingKey(collectionItem);
            return new CollectionEditorItem(key, collectionItem);
        }
        private string GetGroupingKey(ICollectionItem collectionItem)
        {
            if (string.IsNullOrEmpty(GroupBy))
            {
                return null;
            }
            object propertyValue = Reflection.GetPropertyValue(collectionItem, GroupBy);
            if (propertyValue != null)
            {
                return propertyValue.ToString();
            }
            return null;
        }
        protected IEnumerable<ICollectionItem> GetParentCollection()
		{
            return this.Items.Select(t=> t.Item);
		}
		private void MoveItem(ICollectionEditorItem itemToMove, int positionOffset)
		{
			ICollectionEditorItem selectedItem = this.SelectedItem;
			this.Items.Move(this.Items.IndexOf(itemToMove), this.Items.IndexOf(itemToMove) + positionOffset);
			this.SelectedItem = selectedItem;
		}
		private ICollectionEditorItem CloneAndAddItem(ICollectionItem collectionItem)
		{
			ICollectionItem clonedItem = collectionItem.Clone();
			clonedItem.ParentCollection = this.GetParentCollection();
			ICollectionEditorItem collectionEditorItem = CreateEditorItem(clonedItem);
			this.Items.Add(collectionEditorItem);
			return collectionEditorItem;
		}

        private void UpdateDefaultItemTypeAfterAdd(Type addedItemType)
		{
			if (addedItemType == this.DefaultItemType)
			{
				return;
			}
			int itemTypeIndex = Array.IndexOf<Type>(this.AvailableItemTypes, addedItemType);
			this.AvailableItemTypes[itemTypeIndex] = this.DefaultItemType;
			this.DefaultItemType = addedItemType;

			base.RaisePropertyChanged<Type>(() => this.DefaultItemType);
			base.RaisePropertyChanged<Type[]>(() => this.AvailableItemTypes);
		}
		protected override bool CanExecuteOKCommand()
		{
			return base.CanExecuteOKCommand() && this.AreAllItemsValid();
		}
		private bool AreAllItemsValid()
		{
			return this.Items
                .Select(t=> t.Item)
				.All(new Func<ICollectionItem, bool>(Validator.IsValid));
		}
		
		private IObservable<EventPattern<PropertyChangedEventArgs>> ObserveItemPropertyChanges(ICollectionItem collectionItem)
		{
			return Observable
					.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
						addHandler => collectionItem.PropertyChanged += addHandler,
						removeHandler => collectionItem.PropertyChanged -= removeHandler);
		}
		
		private void OnItemPropertyChanged(EventPattern<PropertyChangedEventArgs> eventPattern)
		{
			this.UpdateCommandStates();
		}
		
	}
}
