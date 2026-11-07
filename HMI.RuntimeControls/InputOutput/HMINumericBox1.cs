// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Input;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "UnitElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
	public class HMINumericBox1 : HMINumericBoxBase
	{
		public HMINumericBox1()
		{
			base.DefaultStyleKey=(typeof(HMINumericBox1));
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
