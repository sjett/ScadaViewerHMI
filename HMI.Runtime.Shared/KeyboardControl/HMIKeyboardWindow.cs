// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	public static class HMIKeyboardWindow
	{
		private static bool isWindowOpen;
		public static Panel Container
		{
			get;
			set;
		}
		public static bool IsWindowOpen
		{
			get
			{
				return HMIKeyboardWindow.isWindowOpen;
			}
			set
			{
				HMIKeyboardWindow.isWindowOpen = value;
			}
		}
	}
}
