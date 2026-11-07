// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIAnimatedTriangle1ViewModel : HMIAnimatedPrimitiveBaseViewModel
	{
		public HMIAnimatedTriangle1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIAnimatedTriangle1";
		}
	}
}
