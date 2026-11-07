// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.Common.Presentation
{
	public static class UserInterface
	{
		public static Window? GetActiveWindow()
		{
            return Application.Current.Windows.Cast<Window>().FirstOrDefault(t => t.IsActive);
		}
	}
}
