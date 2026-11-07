// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Modules.Application.Actions;
using HMI.Modules.Build.Resources;

namespace HMI.Modules.Build.Actions
{
	internal class ShowBuildingProjectBusyIndicatorAction : ShowBusyIndicatorAction
	{
		protected override string Message
		{
			get
			{
				return BuildTexts.BuildingProject;
			}
		}
		public ShowBuildingProjectBusyIndicatorAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			if (BuildBusyStatus.IsBusy)
			{
				return false;
			}
			BuildBusyStatus.IsBusy = true;
			return base.Execute(workflow);
		}
	}
}
