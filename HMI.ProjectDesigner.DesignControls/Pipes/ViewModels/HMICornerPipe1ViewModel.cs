// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Pipes.ViewModels
{
	public class HMICornerPipe1ViewModel : PipeControlViewModel
	{
		public HMICornerPipe1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMICornerPipe1";
		}
	}
}
