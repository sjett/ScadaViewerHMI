// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.Common.Presentation
{
	public static class VisualStateSwitcher
	{
		public static bool SwitchVisualState(FrameworkElement control, string state, bool useTransitions)
		{
			return VisualStateManager.GoToState(control, state, useTransitions);
		}
	}
}
