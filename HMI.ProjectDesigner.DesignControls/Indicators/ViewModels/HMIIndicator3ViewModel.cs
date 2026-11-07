// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Indicators.ViewModels
{
	public class HMIIndicator3ViewModel : HMIIndicatorBaseViewModel
	{
		public HMIIndicator3ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIIndicator3";
		}
	}
}
