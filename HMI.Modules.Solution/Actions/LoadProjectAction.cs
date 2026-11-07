// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Modules.Solution.Actions
{
    public class LoadProjectAction : SolutionAction, IWorkflowAction
	{
		public override ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Asynchronous;
			}
		}
		public LoadProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			Contract.Requires<InvalidOperationException>(workflow.Data.ContainsKey("projectFileName"), "Project file name not specified");
			Contract.Requires<InvalidOperationException>(workflow.Data["projectFileName"] is string, "Project file name must be a string");
			string path = (string)workflow.Data["projectFileName"];
			IProjectViewModel value = ProjectManagerService.Value.OpenProject(path);
			workflow.Data["projectKey"] = value;
			return true;
		}
	}
}
