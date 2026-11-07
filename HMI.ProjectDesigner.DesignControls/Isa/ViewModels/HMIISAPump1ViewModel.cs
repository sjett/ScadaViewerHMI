// SPDX-License-Identifier: GPL-3.0-only

using System;

using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAPump1ViewModel : HMIISABaseViewModel
	{
		public HMIISAPump1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAPump1";
		}
	}
}
