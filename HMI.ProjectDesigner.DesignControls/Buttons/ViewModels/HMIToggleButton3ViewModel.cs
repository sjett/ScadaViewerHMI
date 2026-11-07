// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIToggleButton3ViewModel : HMIToggleButtonBaseViewModel
	{
		public HMIToggleButton3ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIToggleButton3";
		}
	}
}
