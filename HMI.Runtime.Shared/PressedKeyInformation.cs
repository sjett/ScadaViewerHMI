// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Input;
namespace HMI.Runtime.Shared
{
	public class PressedKeyInformation
	{
		public Key Key
		{
			get;
			set;
		}
		public ModifierKeys KeyboardModifiers
		{
			get;
			set;
		}
	}
}
