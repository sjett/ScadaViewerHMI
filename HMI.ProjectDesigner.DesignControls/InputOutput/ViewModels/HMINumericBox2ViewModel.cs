// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMINumericBox2ViewModel : HMINumericBoxBaseViewModel
	{
		public HMINumericBox2ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMINumericBox2";
		}
	}
}
