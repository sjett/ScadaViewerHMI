// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.ViewModels;
using System.Collections.ObjectModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reflection;

namespace HMI.Modules.PropertyEditor.ViewModels
{
	public class HorizontalPropertyEditorViewModel : ViewModel, IHorizontalPropertyEditorViewModel
	{

		private readonly IPropertyInspectorService _propertyInspectorService;
		private bool _isColumnRefreshEnabled = true;

		private HorizontalEditorItem _selectedItem;

		private string _groupBy;
		private bool _isCollectionGroupingEnabled;
		private bool _expandAllGroups;
	
		public event EventHandler? UpdateGridViewColumns;

		public event EventHandler<ValueChangedEventArgs<HorizontalEditorItem>>? SelectedItemChanged;

		public event EventHandler<ValueChangedEventArgs<string>>? GroupByChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? IsCollectionGroupingEnabledChanged;

		public event EventHandler<ValueChangedEventArgs<bool>>? AreGroupsExpandedChanged;

		public IList<ColumnDefinition> ViewColumns
		{
			get;
			private set;
		}
		public ObservableCollection<HorizontalEditorItem> Items
		{
			get;
			set;
		}
		IEnumerable<ColumnDefinition> IHorizontalPropertyEditorViewModel.ViewColumns
		{
			get
			{
				return ViewColumns;
			}
		}
		public HorizontalEditorItem SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				if (_selectedItem != value)
				{
					ValueChangedEventArgs<HorizontalEditorItem> e = new ValueChangedEventArgs<HorizontalEditorItem>(_selectedItem, value);
                    _selectedItem = value;
                    SelectedItemChanged?.Invoke(this, e);
					base.RaisePropertyChanged<HorizontalEditorItem>(() => SelectedItem);
				}
			}
		}
		public string GroupBy
		{
			get
			{
				return _groupBy;
			}
			set
			{
				if (_groupBy != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(_groupBy, value);
                    _groupBy = value;
                    GroupByChanged?.Invoke(this, e);
					base.RaisePropertyChanged<string>(() => GroupBy);
				}
			}
		}
		public bool IsCollectionGroupingEnabled
		{
			get
			{
				return _isCollectionGroupingEnabled;
			}
			set
			{
				if (_isCollectionGroupingEnabled != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_isCollectionGroupingEnabled, value);
                    _isCollectionGroupingEnabled = value;
                    IsCollectionGroupingEnabledChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => IsCollectionGroupingEnabled);
				}
			}
		}
		public bool ExpandAllGroups
		{
			get
			{
				return _expandAllGroups;
			}
			set
			{
				if (_expandAllGroups != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_expandAllGroups, value);
                    _expandAllGroups = value;
                    AreGroupsExpandedChanged?.Invoke(this, e);
					base.RaisePropertyChanged<bool>(() => ExpandAllGroups);
				}
			}
		}
		public HorizontalPropertyEditorViewModel(IPropertyInspectorService propertyInspectorService)
		{
			Contract.Requires<ArgumentNullException>(propertyInspectorService != null);

            _propertyInspectorService = propertyInspectorService;
            Items = new ObservableCollection<HorizontalEditorItem>();
            ViewColumns = new List<ColumnDefinition>();
		}
		public void SetDisplayObjects(IEnumerable<object> displayObjects)
		{
            Items.Clear();
            SelectedItem = null;
			if (displayObjects == null)
			{
				return;
			}
            ViewColumns.Clear();
            AddDisplayObjects(displayObjects);
		}
		public void AddObjectToItems(object newItem)
		{
			if (newItem == null)
			{
				return;
			}
			string key = GetItemKey(newItem);
			HorizontalEditorItem horizontalEditorItem = new HorizontalEditorItem
			{
				Value = newItem,
				Key = key
			};
			ICollection<IProperty> properties = _propertyInspectorService.GetProperties(new object[]
			{
				newItem
			});
			if (properties != null)
			{
                foreach (IProperty property in properties)
                {
                    horizontalEditorItem.Properties.Add(property);
                }
			}
            Items.Add(horizontalEditorItem);
            RefreshViewColumns();
		}
		private void AddDisplayObjects(IEnumerable<object> displayObjects)
		{
            _isColumnRefreshEnabled = false;
			foreach (object displayObject in displayObjects)
			{
                AddObjectToItems(displayObject);
			}
            _isColumnRefreshEnabled = true;
            RefreshViewColumns();
		}
		private void SetGroupingKeyFromProperty(object item, ref string groupingKey)
		{
			PropertyInfo property = item.GetType().GetProperty(GroupBy);
			if (property != null)
			{
				object value = property.GetValue(item, null);
				if (value != null)
				{
					groupingKey = value.ToString();
				}
			}
		}
		private string GetItemKey(object item)
		{
			string itemKey = null;
			if (string.IsNullOrEmpty(GroupBy))
			{
				return item.GetType().Name;
			}
            SetGroupingKeyFromProperty(item, ref itemKey);
			return itemKey;
		}
		private void RefreshViewColumns()
		{
			if (!_isColumnRefreshEnabled)
			{
				return;
			}
			foreach (HorizontalEditorItem editorItem in Items)
			{
				for (int propertyIndex = 0; propertyIndex < editorItem.Properties.Count; propertyIndex++)
				{
					IProperty property = editorItem.Properties[propertyIndex];
					if (!ContainsColumn(property))
					{
						AddViewColumn(propertyIndex, property);
					}
				}
			}

			UpdateGridViewColumns?.Invoke(this, EventArgs.Empty);
		}
        private bool ContainsColumn(IProperty property)
        {
			return ViewColumns.Any(column => column.Name == property.Name);
        }
        private void AddViewColumn(int propertyIndex, IProperty property)
        {
            ColumnDefinition column = new ColumnDefinition
            {
                Name = property.Name,
                PropertyIndex = propertyIndex,
                Header = property.DisplayName
            };
            ViewColumns.Add(column);
        }
    }
}
