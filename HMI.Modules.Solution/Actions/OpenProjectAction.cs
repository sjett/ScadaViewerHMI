// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
namespace HMI.Modules.Solution.Actions
{
    public class OpenProjectAction : SolutionAction, IWorkflowAction
	{
		public OpenProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			string value;
			if (!NotificationService.Value.ShowOpenProjectDialog(out value))
			{
				return false;
			}
			workflow.Data["projectFileName"] = value;
			return true;
		}
	}
}
