// SPDX-License-Identifier: GPL-3.0-only

using System;

using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAValveCheck1ViewModel : HMIISABaseViewModel
	{
		public HMIISAValveCheck1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAValveCheck1";
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
		}
	}
}
