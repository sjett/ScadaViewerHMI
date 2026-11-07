// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Input;
namespace HMI.RuntimeControls.InputOutput
{
	public class HMIKeyEventArgs : RoutedEventArgs
	{
		private bool handled;
		private KeyEventArgs keyToWrap;
		private ModifierKeys keyboardModifiers;
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
				this.HandleWrappedKeyEventArgs(this.handled);
			}
		}
		public Key Key
		{
			get;
			set;
		}

		public object OriginalSource
		{
			get;
			set;
		}
		public KeyEventArgs KeyToWrap
		{
			get
			{
				return this.keyToWrap;
			}
			set
			{
				this.keyToWrap = value;
				this.SetKeyWrapper(this.keyToWrap);
			}
		}
		public ModifierKeys KeyboardModifiers
		{
			get
			{
				return this.keyboardModifiers;
			}
			set
			{
				this.keyboardModifiers = value;
			}
		}
		private void HandleWrappedKeyEventArgs(bool handled)
		{
			if (this.KeyToWrap != null)
			{
				this.KeyToWrap.Handled=(handled);
			}
		}
		private void SetKeyWrapper(KeyEventArgs e)
		{
			this.keyToWrap = e;
			this.Key = e.Key;
			this.OriginalSource = e.OriginalSource;
			this.keyboardModifiers = 0;
		}
	}
}
