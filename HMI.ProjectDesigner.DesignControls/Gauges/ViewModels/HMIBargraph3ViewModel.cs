// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Gauges.ViewModels
{
	public class HMIBargraph3ViewModel : HMIBargraphBaseViewModel
	{
		public HMIBargraph3ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIBargraph3";
		}
	}
}
