// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	[TemplatePart(Name = "ButtonsStackPanelElement", Type = typeof(StackPanel))]
	public abstract class MessageBoxBase : DialogControl, IDisposable
	{
		protected const string ElementButtonsStackPanel = "ButtonsStackPanelElement";
		protected StackPanel stkpButtons;
		private readonly IDictionary<Button, RoutedEventHandler> buttonClickHandlers = new Dictionary<Button, RoutedEventHandler>();
		public static readonly DependencyProperty MessageBoxTypeProperty = DependencyProperty.Register("MessageBoxType", typeof(MessageBoxType), typeof(MessageBoxBase), new PropertyMetadata(MessageBoxType.Information));
		public MessageBoxType MessageBoxType
		{
			get
			{
				return (MessageBoxType)base.GetValue(MessageBoxBase.MessageBoxTypeProperty);
			}
			set
			{
				base.SetValue(MessageBoxBase.MessageBoxTypeProperty, value);
			}
		}
		public MessageBoxBase()
		{
            base.ResizeMode = System.Windows.ResizeMode.NoResize;// Telerik.Windows.Controls.ResizeMode.NoResize;
		}
		protected Button AddButton(string buttonText, RoutedEventHandler clickHandler, double spacing = 0.0)
		{
			Button button = new Button();
			button.Content=(buttonText);
			button.MinWidth=(80.0);
			button.Padding=(new Thickness(4.0));
			button.Margin=(new Thickness(spacing, 0.0, 0.0, 0.0));
			Button button2 = button;
			button2.Click +=(clickHandler);
			this.stkpButtons.Children.Add(button2);
			this.buttonClickHandlers.Add(button2, clickHandler);
			return button2;
		}
		protected Button AddButton(object content, RoutedEventHandler clickHandler, double spacing = 0.0)
		{
			Button button = new Button();
			button.Content=(content);
			button.MinWidth=(80.0);
			button.Padding=(new Thickness(4.0));
			button.Margin=(new Thickness(spacing, 0.0, 0.0, 0.0));
			Button button2 = button;
			button2.Click +=(clickHandler);
			this.stkpButtons.Children.Add(button2);
			this.buttonClickHandlers.Add(button2, clickHandler);
			return button2;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
           // base.TabNavigation = System.Windows.Input.KeyboardNavigationMode.Cycle;
			this.stkpButtons = (base.GetTemplateChild("ButtonsStackPanelElement") as StackPanel);
			if (this.stkpButtons != null)
			{
				this.SetupMessageBoxButtons();
			}
		}
		protected abstract void SetupMessageBoxButtons();
		protected override void OnDispose()
		{
			base.OnDispose();
			this.CleanupButtonClickHandlers();
			this.CleanupButtonPanel();
		}
		private void CleanupButtonPanel()
		{
			if (this.stkpButtons != null)
			{
				this.stkpButtons.Children.Clear();
			}
		}
		private void CleanupButtonClickHandlers()
		{
			using (IEnumerator<KeyValuePair<Button, RoutedEventHandler>> enumerator = this.buttonClickHandlers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Button, RoutedEventHandler> current = enumerator.Current;
					current.Key.Click-=(current.Value);
				}
			}
			this.buttonClickHandlers.Clear();
		}
	}
}
