// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.ControlTree
{
	public class ControlMappings : List<ControlCategoryMapping>
	{
		public IEnumerable<ControlCategoryMapping> GetControlsInCategory(string categoryName)
		{
			return this.Where(c => c.CategoryName == categoryName);
		}
	}
}
