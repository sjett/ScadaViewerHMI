// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class GroupAction : GroupOperationAction
	{
		public GroupAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			IProjectPageViewModel activePage = Solution.Value.ActivePage;
			activePage.PromoteSelectionToGroup();
			return true;
		}
	}
}
