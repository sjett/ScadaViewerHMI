// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
namespace HMI.Modules.Solution.Actions
{
    public class SaveProjectAction : SolutionAction, IWorkflowAction
	{
		public SaveProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
            ProjectManagerService.Value.Save(Project);
			return true;
		}
	}
}
