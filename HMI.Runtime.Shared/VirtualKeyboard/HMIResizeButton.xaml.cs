// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
namespace HMI.Runtime.Shared.VirtualKeyboard
{
    public partial class HMIResizeButton : UserControl
	{
		public HMIResizeButton()
		{
			this.InitializeComponent();
		}
		private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.GoToState("Normal");
		}
		private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.GoToState("Pressed");
			if (HMIResizeButton.HasVirtualkeyboad())
			{
				HMIVirtualKeyboard.PrepareForRezise();
				return;
			}
			if (HMIResizeButton.HasNumericKeyboard())
			{
				HMINumericKeyboard.PrepareForRezise();
			}
		}
		private static bool HasVirtualkeyboad()
		{
			return HMIKeyboardWindow.Container.Children.Contains(HMIVirtualKeyboard.HMIKeyboardInstance);
		}
		private static bool HasNumericKeyboard()
		{
			return HMIKeyboardWindow.Container.Children.Contains(HMINumericKeyboard.HMINumericKeyboardInstance);
		}
		private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
		{
			this.GoToState("Normal");
		}
		private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
		{
			this.GoToState("MouseOver");
		}
		private void GoToState(string stateName)
		{
			try
			{
				VisualStateManager.GoToState(this.Button, stateName, true);
			}
			catch
			{
			}
		}
		
	}
}
