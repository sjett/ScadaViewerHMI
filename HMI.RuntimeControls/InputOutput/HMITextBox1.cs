// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.VirtualKeyboard;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "ContentElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates"),
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	public class HMITextBox1 : HMITextBoxBase
	{
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
		public HMITextBox1()
		{
			base.DefaultStyleKey=(typeof(HMITextBox1));
		}
		protected override bool ShouldHandlePressedKey(KeyEventArgs e)
		{
			return base.IsInEditMode || base.IsNavigationKey(e.Key);
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
		protected override void OnUserChangedSignalValue(UserChangedSignalValueEventArgs e)
		{
			base.OnUserChangedSignalValue(e);
			base.PutSelectionAtEnd();
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!base.IsInEditMode)
			{
				base.IsInEditMode = true;
			}
			if (!base.IsInEditMode)
			{
				e.Handled=(!base.IsNavigationKey(e.Key));
			}
			base.OnKeyDown(e);
		}
	}
}
