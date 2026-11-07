// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared.Dialogs;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	public class MessageBoxControl : MessageBoxBase, IDisposable
	{
		public static readonly DependencyProperty MessageBoxControlButtonsProperty = DependencyProperty.Register("Buttons", typeof(MessageBoxControlButtons), typeof(MessageBoxControl), new PropertyMetadata(MessageBoxControlButtons.OK, new PropertyChangedCallback(MessageBoxControl.OnMessageBoxControlButtonsPropertyChanged)));
		public static readonly DependencyProperty ButtonOKTextProperty = DependencyProperty.Register("ButtonOKText", typeof(string), typeof(MessageBoxControl), new PropertyMetadata("OK", new PropertyChangedCallback(MessageBoxControl.OnButtonOKTextPropertyChanged)));
		public static readonly DependencyProperty ButtonCancelTextProperty = DependencyProperty.Register("ButtonCancelText", typeof(string), typeof(MessageBoxControl), new PropertyMetadata("Cancel", new PropertyChangedCallback(MessageBoxControl.OnButtonCancelTextPropertyChanged)));
		public static readonly DependencyProperty ButtonYesTextProperty = DependencyProperty.Register("ButtonYesText", typeof(string), typeof(MessageBoxControl), new PropertyMetadata("Yes", new PropertyChangedCallback(MessageBoxControl.OnButtonYesTextPropertyChanged)));
		public static readonly DependencyProperty ButtonNoTextProperty = DependencyProperty.Register("ButtonNoText", typeof(string), typeof(MessageBoxControl), new PropertyMetadata("No", new PropertyChangedCallback(MessageBoxControl.OnButtonNoTextPropertyChanged)));
		
		public new event EventHandler<DialogClosedEventArgs> DialogClosed;
		
		[Category("HMI")]
		public MessageBoxControlButtons Buttons
		{
			get
			{
				return (MessageBoxControlButtons)base.GetValue(MessageBoxControl.MessageBoxControlButtonsProperty);
			}
			set
			{
				base.SetValue(MessageBoxControl.MessageBoxControlButtonsProperty, value);
			}
		}
		[Category("HMI")]
		public string ButtonOKText
		{
			get
			{
				return (string)base.GetValue(MessageBoxControl.ButtonOKTextProperty);
			}
			set
			{
				base.SetValue(MessageBoxControl.ButtonOKTextProperty, value);
			}
		}
		[Category("HMI")]
		public string ButtonCancelText
		{
			get
			{
				return (string)base.GetValue(MessageBoxControl.ButtonCancelTextProperty);
			}
			set
			{
				base.SetValue(MessageBoxControl.ButtonCancelTextProperty, value);
			}
		}
		[Category("HMI")]
		public string ButtonYesText
		{
			get
			{
				return (string)base.GetValue(MessageBoxControl.ButtonYesTextProperty);
			}
			set
			{
				base.SetValue(MessageBoxControl.ButtonYesTextProperty, value);
			}
		}
		[Category("HMI")]
		public string ButtonNoText
		{
			get
			{
				return (string)base.GetValue(MessageBoxControl.ButtonNoTextProperty);
			}
			set
			{
				base.SetValue(MessageBoxControl.ButtonNoTextProperty, value);
			}
		}
		private static void OnMessageBoxControlButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnButtonOKTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxControl messageBoxControl = d as MessageBoxControl;
			if (messageBoxControl != null && messageBoxControl.stkpButtons != null && (messageBoxControl.Buttons == MessageBoxControlButtons.OK || messageBoxControl.Buttons == MessageBoxControlButtons.OKCancel))
			{
				((Button)messageBoxControl.stkpButtons.Children[0]).Content=(e.NewValue);
			}
		}
		private static void OnButtonCancelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxControl messageBoxControl = d as MessageBoxControl;
			if (messageBoxControl != null && messageBoxControl.stkpButtons != null && messageBoxControl.Buttons == MessageBoxControlButtons.OKCancel)
			{
				((Button)messageBoxControl.stkpButtons.Children[1]).Content=(e.NewValue);
			}
		}
		private static void OnButtonYesTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxControl messageBoxControl = d as MessageBoxControl;
			if (messageBoxControl != null && messageBoxControl.stkpButtons != null && messageBoxControl.Buttons == MessageBoxControlButtons.YesNo)
			{
				((Button)messageBoxControl.stkpButtons.Children[0]).Content=(e.NewValue);
			}
		}
		private static void OnButtonNoTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MessageBoxControl messageBoxControl = d as MessageBoxControl;
			if (messageBoxControl != null && messageBoxControl.stkpButtons != null && messageBoxControl.Buttons == MessageBoxControlButtons.YesNo)
			{
				((Button)messageBoxControl.stkpButtons.Children[1]).Content=(e.NewValue);
			}
		}
		public MessageBoxControl()
		{
			base.DefaultStyleKey=(typeof(MessageBoxControl));
		}
		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			base.Close(Dialogs.DialogResult.OK);
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
            base.Close(Dialogs.DialogResult.Cancel);
		}
		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
            base.Close(Dialogs.DialogResult.Yes);
		}
		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
            base.Close(Dialogs.DialogResult.No);
		}
		protected override void SetupMessageBoxButtons()
		{
			switch (this.Buttons)
			{
			case MessageBoxControlButtons.OK:
				this.AddOKButton();
				return;
			case MessageBoxControlButtons.OKCancel:
				this.AddOKButton();
				this.AddCancelButton();
				return;
			case MessageBoxControlButtons.YesNo:
				this.AddYesButton();
				this.AddNoButton();
				return;
			default:
				throw new ArgumentOutOfRangeException("MessageBoxControlButtons");
			}
		}
		private void AddNoButton()
		{
			base.AddButton(this.ButtonNoText, new RoutedEventHandler(this.NoButton_Click), 10.0);
		}
		private void AddYesButton()
		{
			base.AddButton(this.ButtonYesText, new RoutedEventHandler(this.YesButton_Click), 0.0);
		}
		private void AddCancelButton()
		{
			base.AddButton(this.ButtonCancelText, new RoutedEventHandler(this.CancelButton_Click), 10.0);
		}
		private void AddOKButton()
		{
			base.AddButton(this.ButtonOKText, new RoutedEventHandler(this.OKButton_Click), 0.0);
		}
	}
}
