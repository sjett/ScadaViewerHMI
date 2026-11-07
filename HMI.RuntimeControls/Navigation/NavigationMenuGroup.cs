// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;

namespace HMI.RuntimeControls.Navigation
{

    internal class NavigationMenuGroup
	{
		static Dictionary<string, List<HMINavigationToggle1>> menuGroups = new Dictionary<string, List<HMINavigationToggle1>>(StringComparer.InvariantCultureIgnoreCase);

		public static void TryAdd(HMINavigationToggle1 navigationToggle1)
		{
            List<HMINavigationToggle1> group;
			if (menuGroups.ContainsKey(navigationToggle1.MenuGroup))
			{
				group = menuGroups[navigationToggle1.MenuGroup];
			}
			else
			{
				group = new List<HMINavigationToggle1>();
				menuGroups.Add(navigationToggle1.MenuGroup, group);
			}

            if (!group.Contains(navigationToggle1))
			{
				group.Add(navigationToggle1);
			}
		}

		public static void UncheckOtherNavToggleInGroup(HMINavigationToggle1 navigationToggle1)
		{
			if (menuGroups.TryGetValue(navigationToggle1.MenuGroup, out var group))
			{
				foreach (HMINavigationToggle1 navToggle1 in group)
				{
					if (!navToggle1.Equals(navigationToggle1))
					{
						navToggle1.Checked = false;
					}
				}
			}
		}
		public static bool TryRemove(HMINavigationToggle1 navigationToggle1)
		{
			bool result = false;
			if (menuGroups.TryGetValue(navigationToggle1.MenuGroup, out var group))
            {
				result = group.Remove(navigationToggle1);
			}
			return result;
		}
	}
}
