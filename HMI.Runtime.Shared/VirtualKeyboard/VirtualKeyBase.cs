// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Shared.KeyboardControl;
namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public abstract class VirtualKeyBase : UserControl
	{
		public event VirtualKeyPressedEventHandler VirtualKeyPressed;
		protected Button KeyButton
		{
			get;
			set;
		}
		public int KeyNum
		{
			get;
			set;
		}
		public string Text
		{
			get
			{
				return this.KeyButton.Content.ToString();
			}
			set
			{
				this.KeyButton.Content=(value);
			}
		}

		protected abstract string KeyString{get;}
		
        public VirtualKeyBase()
		{
			base.MouseEnter+=(new MouseEventHandler(this.KeyButton_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.KeyButton_MouseLeave));
			base.MouseLeftButtonUp+=(new MouseButtonEventHandler(this.KeyButton_MouseLeftButtonUp));
			base.MouseLeftButtonDown+=(new MouseButtonEventHandler(this.KeyButton_MouseLeftButtonDown));
		}
		protected void KeyButton_MouseEnter(object sender, MouseEventArgs e)
		{
			this.GoToState("MouseOver");
		}
		protected void KeyButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this.GoToState("Normal");
		}
		protected void KeyButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.GoToState("MouseOver");
		}
		protected void KeyButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
		{
			this.GoToState("Pressed");
			this.OnVirtualKeyPressed(this.KeyString);
		}
		private void GoToState(string stateName)
		{
			VisualStateManager.GoToState(this.KeyButton, stateName, true);
		}
		protected virtual void OnVirtualKeyPressed(string pressedKey)
		{
			if (this.VirtualKeyPressed != null)
			{
				this.VirtualKeyPressed(this, new VirtualKeyPressedEventArgs(pressedKey));
			}
		}
	}
}
