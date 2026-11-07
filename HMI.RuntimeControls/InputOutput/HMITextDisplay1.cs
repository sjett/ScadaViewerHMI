// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "OutputElement", Type = typeof(TextBlock)), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit3"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit4")]
	public class HMITextDisplay1 : HMIDisplayBase
	{
		public HMITextDisplay1()
		{
			base.DefaultStyleKey=(typeof(HMITextDisplay1));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
		protected override void OnUpdateOutput()
		{
			if (base.OutputElement == null)
			{
				return;
			}
			try
			{
				base.OutputElement.Text=(base.SignalValue.ToString());
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
