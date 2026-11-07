// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public interface IVirtualKeyboardAware : IDisposable
	{
		VirtualKeyboardSettings VirtualKeyboardSettings
		{
			get;
		}
		void SendKey(string key);
	}
}
