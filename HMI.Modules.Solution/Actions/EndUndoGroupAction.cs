// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Solution.Actions
{
    public class EndUndoGroupAction : IWorkflowAction
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public EndUndoGroupAction(IServiceProvider serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		public bool Execute(IWorkflow workflow)
		{
            _projectManager.Value.Project.ActionMonitor.EndGrouping();
			return true;
		}
	}
}
