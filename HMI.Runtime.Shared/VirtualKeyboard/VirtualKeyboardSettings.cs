// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared.KeyboardControl;
using System;
using System.Windows;
namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public class VirtualKeyboardSettings
	{
		public VirtualKeyboardType VirtualKeyboardType
		{
			get;
			set;
		}
		public VirtualKeyboardControlActivation Activation
		{
			get;
			private set;
		}
		public VirtualKeyboardSettings(FrameworkElement owner, VirtualKeyboardControlActivation activation)
		{
			this.Activation = activation;
			if (activation != VirtualKeyboardControlActivation.Manual)
			{
				KeyboardService.SetVirtualKeyboardType(owner, VirtualKeyboardType.Alphanumeric);
			}
		}
		internal VirtualKeyboardSettings()
		{
			this.Activation = VirtualKeyboardControlActivation.OnFocus;
		}
	}
}
