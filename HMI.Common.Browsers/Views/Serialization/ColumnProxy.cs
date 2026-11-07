// SPDX-License-Identifier: GPL-3.0-only
using Telerik.Windows.Controls;
namespace HMI.Common.Browsers.Views.Serialization
{
	public class ColumnProxy
	{
		public string UniqueName
		{
			get;
			set;
		}
		public int DisplayOrder
		{
			get;
			set;
		}
		public string Header
		{
			get;
			set;
		}
		public GridViewLength Width
		{
			get;
			set;
		}
	}
}
