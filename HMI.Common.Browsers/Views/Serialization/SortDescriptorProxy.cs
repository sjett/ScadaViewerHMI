// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;
namespace HMI.Common.Browsers.Views.Serialization
{
	public class SortDescriptorProxy
	{
		public string ColumnUniqueName
		{
			get;
			set;
		}
		public ListSortDirection SortDirection
		{
			get;
			set;
		}
	}
}
