// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.Generic;
using Telerik.Windows.Data;
namespace HMI.Common.Browsers.Views.Serialization
{
	public class FilterSetting
	{
		private List<object> selectedDistinctValues;
		public string ColumnUniqueName
		{
			get;
			set;
		}
		public List<object> SelectedDistinctValues
		{
			get
			{
				if (this.selectedDistinctValues == null)
				{
					this.selectedDistinctValues = new List<object>();
				}
				return this.selectedDistinctValues;
			}
		}
		public FilterDescriptorProxy Filter1
		{
			get;
			set;
		}
		public FilterCompositionLogicalOperator FieldFilterLogicalOperator
		{
			get;
			set;
		}
		public FilterDescriptorProxy Filter2
		{
			get;
			set;
		}
	}
}
