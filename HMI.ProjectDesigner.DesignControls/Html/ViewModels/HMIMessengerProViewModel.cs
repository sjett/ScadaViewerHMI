// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Html.ViewModels
{
	public class HMIMessengerProViewModel : HtmlBaseViewModel
	{
		public HMIMessengerProViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIMessengerPro";
		}
	}
}
