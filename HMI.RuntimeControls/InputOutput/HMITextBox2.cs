// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.VirtualKeyboard;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "CancelElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "ContentElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ApplyElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "EditElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "NormalMode", GroupName = "EditStates"), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "EditingMode", GroupName = "EditStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
	public class HMITextBox2 : HMITextBoxBase
	{
		private const string VisualStateGroupEditStates = "EditStates";
		private const string VisualStateNormalMode = "NormalMode";
		private const string VisualStateEditingMode = "EditingMode";
		private const string ApplyElement = "ApplyElement";
		private const string CancelElement = "CancelElement";
		private const string EditElement = "EditElement";

		private ButtonBase? applyElement;
		private ButtonBase? cancelElement;
		private ButtonBase? editElement;

		private CultureInfo cultureInfo;
		private char numberDecimalSeparator;
		private char negativeSign;
		private VirtualKeyboardSettings virtualKeyboardSettings;
		public override VirtualKeyboardSettings VirtualKeyboardSettings
		{
			get
			{
				if (this.virtualKeyboardSettings == null)
				{
					this.virtualKeyboardSettings = new VirtualKeyboardSettings(this, VirtualKeyboardControlActivation.Manual);
					this.virtualKeyboardSettings.VirtualKeyboardType = VirtualKeyboardType.Alphanumeric;
				}
				return this.virtualKeyboardSettings;
			}
		}
		protected override bool VKActiveOnEditModeOnly
		{
			get
			{
				return true;
			}
		}
		public HMITextBox2()
		{
			base.DefaultStyleKey=(typeof(HMITextBox2));
		}
		private void ApplyElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			if (base.IsAuthorized && !string.IsNullOrEmpty(base.SignalName))
			{
				try
				{
					base.WriteSignal(base.ServerName, base.SignalName, base.Text);
					this.OnUpdateOutput();
				}
				catch (Exception ex)
				{
					base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
			base.IsInEditMode = false;
		}
		private void CancelElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			base.IsInEditMode = false;
			this.OnUpdateOutput();
		}
		private void EditElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			base.IsInEditMode = true;
		}
		public override void OnApplyTemplate()
		{
			this.applyElement = null;
			this.cancelElement = null;
			this.editElement = null;
			this.SetCultureVariables();
			base.OnApplyTemplate();
			this.applyElement = (base.GetTemplateChild("ApplyElement") as ButtonBase);
			if (this.applyElement != null)
			{
				this.applyElement.IsTabStop=(false);
				this.applyElement.Click+=(new RoutedEventHandler(this.ApplyElement_Click));
			}
			this.cancelElement = (base.GetTemplateChild("CancelElement") as ButtonBase);
			if (this.cancelElement != null)
			{
				this.cancelElement.IsTabStop=(false);
				this.cancelElement.Click+=(new RoutedEventHandler(this.CancelElement_Click));
			}
			this.editElement = (base.GetTemplateChild("EditElement") as ButtonBase);
			if (this.editElement != null)
			{
				this.editElement.IsTabStop=(false);
				this.editElement.Click+=(new RoutedEventHandler(this.EditElement_Click));
			}
		}
		private void SetCultureVariables()
		{
			this.cultureInfo = CultureInfo.CurrentCulture;
			this.numberDecimalSeparator = this.cultureInfo.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
			this.negativeSign = this.cultureInfo.NumberFormat.NegativeSign.ToCharArray()[0];
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!base.IsInEditMode)
			{
				e.Handled=(!base.IsNavigationKey(e.Key));
			}
			base.OnKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (base.IsInEditMode && e.Key ==  Key.Escape)
			{
				base.IsInEditMode = false;
				this.OnUpdateOutput();
				e.Handled=(true);
			}
		}
		public override void OnUpdateOutput()
		{
			if (base.SignalValue == null)
			{
				base.Text=(string.Empty);
				return;
			}
			try
			{
				base.Text=(Convert.ToString(base.SignalValue, base.CurrentCultureInfo));
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void ChangeVisualState()
		{
			if (base.IsInEditMode)
			{
				VisualStateManager.GoToState(this, "EditingMode", true);
				base.SelectAll();
				return;
			}
			VisualStateManager.GoToState(this, "NormalMode", true);
			if (base.SelectionLength > 0)
			{
				base.SelectionLength=(0);
			}
		}
		protected override void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
		{
			base.OnUserChangedSignalValue(e);
			if (this.HasKeyboardAttached())
			{
				base.PutSelectionAtEnd();
			}
		}
		public bool HasKeyboardAttached()
		{
			return HMITextBox2.HasNumericKeyboard() || HMITextBox2.HasVirtualkeyboard();
		}
		private static bool HasNumericKeyboard()
		{
			return HMIKeyboardWindow.Container != null && HMIKeyboardWindow.Container.Children.Contains(HMINumericKeyboard.HMINumericKeyboardInstance);
		}
		private static bool HasVirtualkeyboard()
		{
			return HMIKeyboardWindow.Container != null && HMIKeyboardWindow.Container.Children.Contains(HMIVirtualKeyboard.HMIKeyboardInstance);
		}
		protected override void OnIsInEditModeChanged()
		{
			base.OnIsInEditModeChanged();
			this.ChangeVisualState();
		}
	}
}
