// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Windows;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels
{
	public class HMITextBox2ViewModel : HMITextBoxBaseViewModel
	{
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(120.0, 50.0);
			}
		}
		public HMITextBox2ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMITextBox2";
		}
	}
}
