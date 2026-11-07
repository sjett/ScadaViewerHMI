// SPDX-License-Identifier: GPL-3.0-only
using Telerik.Windows.Data;
namespace HMI.Common.Browsers.Views.Serialization
{
	public class FilterDescriptorProxy
	{
		public FilterOperator Operator
		{
			get;
			set;
		}
		public object Value
		{
			get;
			set;
		}
		public bool IsCaseSensitive
		{
			get;
			set;
		}
	}
}
