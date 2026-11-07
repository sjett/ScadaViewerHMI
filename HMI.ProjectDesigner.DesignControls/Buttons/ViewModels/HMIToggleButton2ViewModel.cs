// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIToggleButton2ViewModel : HMIToggleButtonBaseViewModel
	{
		public HMIToggleButton2ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIToggleButton2";
		}
	}
}
