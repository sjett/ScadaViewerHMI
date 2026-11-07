// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.InputOutput
{
	public interface IKeyPressedHandleChecker
	{
		TextBoxInformation TextBoxInformation
		{
			get;
			set;
		}
		NumberFormat NumberFormat
		{
			get;
			set;
		}
		bool ShouldHandlePressedKey(PressedKeyInformation keyInformation);
	}
}
