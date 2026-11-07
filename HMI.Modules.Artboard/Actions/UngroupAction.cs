// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Runtime.CompilerServices;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class UngroupAction : IWorkflowAction
	{
		protected Lazy<IProjectManagerViewModel> Solution
		{
			get;
			private set;
		}
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public UngroupAction(IServiceProvider serviceLocator)
		{
            Solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		public bool Execute(IWorkflow workflow)
		{
			IProjectPageViewModel activePage = Solution.Value.ActivePage;
			activePage.DemoteGroupToSelection(activePage.SelectedItem as IControlGroupContainerViewModel);
			return true;
		}
	}
}
