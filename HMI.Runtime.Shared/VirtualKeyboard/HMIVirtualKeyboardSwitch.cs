// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared.KeyboardControl;

namespace HMI.Runtime.Shared.VirtualKeyboard
{
	public class HMIVirtualKeyboardSwitch : Button
	{
		private bool templateApplied;
		public static readonly DependencyProperty KeyboardOnProperty = DependencyProperty.Register("KeyboardOn", typeof(bool), typeof(HMIVirtualKeyboardSwitch), new PropertyMetadata(KeyboardController.Instance.IsUsingVirtualKeyboard, new PropertyChangedCallback(OnKeyboardOnPropertyChanged)));
		public static readonly DependencyProperty IndicatorBackgroundProperty = DependencyProperty.Register("IndicatorBackground", typeof(Brush), typeof(HMIVirtualKeyboardSwitch), new PropertyMetadata(null, new PropertyChangedCallback(OnIndicatorBackgroundPropertyChanged)));
		private bool skipIsUsingVirtualKeyboardChanged;
		private Button uxOnIndicator;

		[Category("HMI")]
		public bool KeyboardOn
		{
			get
			{
				return (bool)GetValue(KeyboardOnProperty);
			}
			set
			{
				SetValue(KeyboardOnProperty, value);
			}
		}
		[Category("HMI")]
		public Brush IndicatorBackground
		{
			get
			{
				return (Brush)GetValue(IndicatorBackgroundProperty);
			}
			set
			{
				SetValue(IndicatorBackgroundProperty, value);
			}
		}
		public HMIVirtualKeyboardSwitch()
		{
			DefaultStyleKey=typeof(HMIVirtualKeyboardSwitch);
		}
		private static void OnKeyboardOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIVirtualKeyboardSwitch wFVirtualKeyboardSwitch = d as HMIVirtualKeyboardSwitch;
			wFVirtualKeyboardSwitch.SetKeyboardOn();
		}
		private static void OnIndicatorBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIVirtualKeyboardSwitch wFVirtualKeyboardSwitch = d as HMIVirtualKeyboardSwitch;
			wFVirtualKeyboardSwitch.SetIndicatorHighlight();
		}
		private void SetKeyboardOn()
		{
			if (templateApplied)
			{
				SetIndicatorHighlight();
				skipIsUsingVirtualKeyboardChanged = true;
				KeyboardController.Instance.IsUsingVirtualKeyboard = KeyboardOn;
				skipIsUsingVirtualKeyboardChanged = false;
			}
		}
		protected override void OnClick()
		{
			base.OnClick();
			KeyboardOn = !KeyboardOn;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}
			if (KeyboardController.Instance.MachineSettingsApplied)
			{
				KeyboardOn = KeyboardController.Instance.IsUsingVirtualKeyboard;
			}
			else
			{
				KeyboardController.Instance.IsUsingVirtualKeyboard = KeyboardOn;
			}
			uxOnIndicator = GetTemplateChild("uxOnIndicator") as Button;
			if (uxOnIndicator != null)
			{
				SetIndicatorHighlight();
				KeyboardController.Instance.IsUsingVirtualKeyboardChanged += new EventHandler(KeyboardController_IsUsingVirtualKeyboardChanged);
			}
			templateApplied = true;
		}
		private void KeyboardController_IsUsingVirtualKeyboardChanged(object sender, EventArgs e)
		{
			if (!skipIsUsingVirtualKeyboardChanged)
			{
				KeyboardOn = KeyboardController.Instance.IsUsingVirtualKeyboard;
			}
		}
		private void SetIndicatorHighlight()
		{
			if (uxOnIndicator != null)
			{
				if (KeyboardOn)
				{
					uxOnIndicator.Background=IndicatorBackground;
					return;
				}
				uxOnIndicator.Background=new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			}
		}
	}
}
