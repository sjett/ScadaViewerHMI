// SPDX-License-Identifier: GPL-3.0-only

using System;

using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAHeatExchanger4ViewModel : HMIISABaseViewModel
	{
		public HMIISAHeatExchanger4ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAHeatExchanger4";
		}
	}
}
