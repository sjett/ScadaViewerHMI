// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Controls;
namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public interface IVirtualKeyboard
	{
		IVirtualKeyboardAware Owner
		{
			get;
			set;
		}
		Control KeyboardControl
		{
			get;
		}
		void ShowKeyboard();
		void HideKeyboard();
		void ToggleResizeControl();
	}
}
