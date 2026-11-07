// SPDX-License-Identifier: GPL-3.0-only

using System;

using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAHalfPipeMixingVessel1ViewModel : HMIISABaseViewModel
	{
		public HMIISAHalfPipeMixingVessel1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAHalfPipeMixingVessel1";
		}
	}
}
