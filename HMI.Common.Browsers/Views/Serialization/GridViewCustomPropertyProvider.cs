// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;
using Telerik.Windows.Persistence.Services;
namespace HMI.Common.Browsers.Views.Serialization
{
	public class GridViewCustomPropertyProvider : ICustomPropertyProvider, IPersistenceProvider
	{
		public CustomPropertyInfo[] GetCustomProperties()
		{
			return new CustomPropertyInfo[]
			{
				new CustomPropertyInfo("Columns", typeof(List<ColumnProxy>)),
				new CustomPropertyInfo("SortDescriptors", typeof(List<SortDescriptorProxy>)),
				new CustomPropertyInfo("GroupDescriptors", typeof(List<GroupDescriptorProxy>)),
				new CustomPropertyInfo("FilterDescriptors", typeof(List<FilterSetting>))
			};
		}
		public void InitializeObject(object context)
		{
			if (context is RadGridView)
			{
				RadGridView radGridView = context as RadGridView;
				radGridView.SortDescriptors.Clear();
				radGridView.GroupDescriptors.Clear();
				List<GridViewColumn> filteredColumns = radGridView.Columns
                    .OfType<GridViewColumn>()
                    .Where(column => column.ColumnFilterDescriptor.IsActive)
                    .ToList();
				filteredColumns.ForEach(column => column.ClearFilters());
			}
		}
		public object InitializeValue(CustomPropertyInfo customPropertyInfo, object context)
		{
			return null;
		}
		public object ProvideValue(CustomPropertyInfo customPropertyInfo, object context)
		{
			RadGridView radGridView = context as RadGridView;
			string name;
			if ((name = customPropertyInfo.Name) != null)
			{
				if (name == "Columns")
				{
					List<ColumnProxy> columns = new List<ColumnProxy>();
					foreach (GridViewColumn column in radGridView.Columns)
					{
						columns.Add(new ColumnProxy
						{
							UniqueName = column.UniqueName,
							Header = column.Header.ToString(),
							DisplayOrder = column.DisplayIndex,
							Width = column.Width
						});
					}
					return columns;
				}
				if (name == "SortDescriptors")
				{
					List<SortDescriptorProxy> sortDescriptors = new List<SortDescriptorProxy>();
					using (IEnumerator<ISortDescriptor> sortDescriptorEnumerator = radGridView.SortDescriptors.GetEnumerator())
					{
						while (sortDescriptorEnumerator.MoveNext())
						{
							ColumnSortDescriptor sortDescriptor = (ColumnSortDescriptor)sortDescriptorEnumerator.Current;
							sortDescriptors.Add(new SortDescriptorProxy
							{
								ColumnUniqueName = sortDescriptor.Column.UniqueName,
								SortDirection = sortDescriptor.SortDirection
							});
						}
					}
					return sortDescriptors;
				}
				if (name == "GroupDescriptors")
				{
					List<GroupDescriptorProxy> groupDescriptors = new List<GroupDescriptorProxy>();
					using (IEnumerator<IGroupDescriptor> groupDescriptorEnumerator = radGridView.GroupDescriptors.GetEnumerator())
					{
						while (groupDescriptorEnumerator.MoveNext())
						{
							ColumnGroupDescriptor groupDescriptor = (ColumnGroupDescriptor)groupDescriptorEnumerator.Current;
							groupDescriptors.Add(new GroupDescriptorProxy
							{
								ColumnUniqueName = groupDescriptor.Column.UniqueName,
								SortDirection = groupDescriptor.SortDirection
							});
						}
					}
					return groupDescriptors;
				}
				if (name == "FilterDescriptors")
				{
					List<FilterSetting> filterSettings = new List<FilterSetting>();
					using (IEnumerator<IFilterDescriptor> filterDescriptorEnumerator = radGridView.FilterDescriptors.GetEnumerator())
					{
						while (filterDescriptorEnumerator.MoveNext())
						{
							IColumnFilterDescriptor columnFilterDescriptor = (IColumnFilterDescriptor)filterDescriptorEnumerator.Current;
							FilterSetting filterSetting = new FilterSetting();
							filterSetting.ColumnUniqueName = columnFilterDescriptor.Column.UniqueName;
							filterSetting.SelectedDistinctValues.AddRange(columnFilterDescriptor.DistinctFilter.DistinctValues);
							if (columnFilterDescriptor.FieldFilter.Filter1.IsActive)
							{
								filterSetting.Filter1 = new FilterDescriptorProxy();
								filterSetting.Filter1.Operator = columnFilterDescriptor.FieldFilter.Filter1.Operator;
								filterSetting.Filter1.Value = columnFilterDescriptor.FieldFilter.Filter1.Value;
								filterSetting.Filter1.IsCaseSensitive = columnFilterDescriptor.FieldFilter.Filter1.IsCaseSensitive;
							}
							filterSetting.FieldFilterLogicalOperator = columnFilterDescriptor.FieldFilter.LogicalOperator;
							if (columnFilterDescriptor.FieldFilter.Filter2.IsActive)
							{
								filterSetting.Filter2 = new FilterDescriptorProxy();
								filterSetting.Filter2.Operator = columnFilterDescriptor.FieldFilter.Filter2.Operator;
								filterSetting.Filter2.Value = columnFilterDescriptor.FieldFilter.Filter2.Value;
								filterSetting.Filter2.IsCaseSensitive = columnFilterDescriptor.FieldFilter.Filter2.IsCaseSensitive;
							}
							filterSettings.Add(filterSetting);
						}
					}
					return filterSettings;
				}
			}
			return null;
		}
		public void RestoreValue(CustomPropertyInfo customPropertyInfo, object context, object value)
		{
			RadGridView radGridView = context as RadGridView;
			string name;
			if ((name = customPropertyInfo.Name) != null)
			{
				if (!(name == "Columns"))
				{
					if (name == "SortDescriptors")
					{
						radGridView.SortDescriptors.SuspendNotifications();
						radGridView.SortDescriptors.Clear();
						List<SortDescriptorProxy> sortDescriptors = value as List<SortDescriptorProxy>;
						foreach (SortDescriptorProxy sortDescriptor in sortDescriptors)
						{
							GridViewColumn column = radGridView.Columns[sortDescriptor.ColumnUniqueName];
							radGridView.SortDescriptors.Add(new ColumnSortDescriptor
							{
								Column = column,
								SortDirection = sortDescriptor.SortDirection
							});
						}
						radGridView.SortDescriptors.ResumeNotifications();
						return;
					}
					if (name == "GroupDescriptors")
					{
						radGridView.GroupDescriptors.SuspendNotifications();
						radGridView.GroupDescriptors.Clear();
						List<GroupDescriptorProxy> groupDescriptors = value as List<GroupDescriptorProxy>;
						foreach (GroupDescriptorProxy groupDescriptor in groupDescriptors)
						{
							GridViewColumn column = radGridView.Columns[groupDescriptor.ColumnUniqueName];
							radGridView.GroupDescriptors.Add(new ColumnGroupDescriptor
							{
								Column = column,
								SortDirection = groupDescriptor.SortDirection
							});
						}
						radGridView.GroupDescriptors.ResumeNotifications();
						return;
					}
					if (!(name == "FilterDescriptors"))
					{
						return;
					}
					radGridView.FilterDescriptors.SuspendNotifications();
					foreach (GridViewColumn column in radGridView.Columns)
					{
						if (column.ColumnFilterDescriptor.IsActive)
						{
							column.ClearFilters();
						}
					}
					List<FilterSetting> filterSettings = value as List<FilterSetting>;
					foreach (FilterSetting filterSetting in filterSettings)
					{
						GridViewColumn gridViewColumn = radGridView.Columns[filterSetting.ColumnUniqueName];
						IColumnFilterDescriptor columnFilterDescriptor = gridViewColumn.ColumnFilterDescriptor;
						foreach (object selectedValue in filterSetting.SelectedDistinctValues)
						{
							columnFilterDescriptor.DistinctFilter.AddDistinctValue(selectedValue);
						}
						if (filterSetting.Filter1 != null)
						{
							columnFilterDescriptor.FieldFilter.Filter1.Operator = filterSetting.Filter1.Operator;
							columnFilterDescriptor.FieldFilter.Filter1.Value = filterSetting.Filter1.Value;
							columnFilterDescriptor.FieldFilter.Filter1.IsCaseSensitive = filterSetting.Filter1.IsCaseSensitive;
						}
						columnFilterDescriptor.FieldFilter.LogicalOperator = filterSetting.FieldFilterLogicalOperator;
						if (filterSetting.Filter2 != null)
						{
							columnFilterDescriptor.FieldFilter.Filter2.Operator = filterSetting.Filter2.Operator;
							columnFilterDescriptor.FieldFilter.Filter2.Value = filterSetting.Filter2.Value;
							columnFilterDescriptor.FieldFilter.Filter2.IsCaseSensitive = filterSetting.Filter2.IsCaseSensitive;
						}
					}
					radGridView.FilterDescriptors.ResumeNotifications();
				}
				else
				{
					List<ColumnProxy> columns = value as List<ColumnProxy>;
					foreach (ColumnProxy columnProxy in columns)
					{
						GridViewColumn column = radGridView.Columns[columnProxy.UniqueName];
						column.DisplayIndex = columnProxy.DisplayOrder;
						column.Header = columnProxy.Header;
						column.Width = columnProxy.Width;
					}
				}
			}
		}
		
	}
}
