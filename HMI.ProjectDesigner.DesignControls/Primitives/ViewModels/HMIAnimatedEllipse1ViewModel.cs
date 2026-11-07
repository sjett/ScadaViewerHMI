// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Primitives.ViewModels
{
	public class HMIAnimatedEllipse1ViewModel : HMIAnimatedPrimitiveBaseViewModel
	{
		public HMIAnimatedEllipse1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIAnimatedEllipse1";
		}
	}
}
