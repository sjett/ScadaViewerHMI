// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.RuntimeControls.Indicators
{
	[
		TemplateVisualState(GroupName = "CommonStates", Name = "State3"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State8"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State6"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State7"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State5"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
	public class HMIIndicator3 : HMIIndicatorBase
	{
		public HMIIndicator3()
		{
			base.DefaultStyleKey=(typeof(HMIIndicator3));
		}
	}
}
