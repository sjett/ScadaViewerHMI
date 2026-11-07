// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Alarms.ViewModels
{
	public class HMIAlarmDuration1ViewModel : HMIHighCountAlarmStatisticBaseViewModel
	{
		public HMIAlarmDuration1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIAlarmDuration1";
		}
	}
}
