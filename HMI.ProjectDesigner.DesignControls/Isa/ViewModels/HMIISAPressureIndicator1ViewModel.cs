// SPDX-License-Identifier: GPL-3.0-only

using System;

using System.Windows;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Isa.ViewModels
{
	
	public class HMIISAPressureIndicator1ViewModel : HMIISABaseViewModel
	{
        public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 50.0);
			}
		}
		public HMIISAPressureIndicator1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIISAPressureIndicator1";
		}
	}
}
