// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.ControlTree
{
	public class ControlTreeControlItem : ControlTreeItem
	{
		
		public object ControlInstance
		{
			get;
			set;
		}
		public ControlTreeControlItem()
		{
		}
		public ControlTreeControlItem(string itemName) : base(itemName)
		{
		}
	}
}
