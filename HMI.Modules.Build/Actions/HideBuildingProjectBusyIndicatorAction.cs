// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Modules.Application.Actions;
namespace HMI.Modules.Build.Actions
{
	public class HideBuildingProjectBusyIndicatorAction : HideBusyIndicatorAction
	{
		public HideBuildingProjectBusyIndicatorAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			BuildBusyStatus.IsBusy = false;
			return base.Execute(workflow);
		}
	}
}
