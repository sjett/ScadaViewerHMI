// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Modules.Solution.Actions
{
    public class ActivateProjectAction : SolutionAction, IWorkflowAction
	{
		public override ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public ActivateProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			Contract.Requires<InvalidOperationException>(workflow.Data.ContainsKey("projectKey"), "Project not specified");
			Contract.Requires<InvalidOperationException>(workflow.Data["projectKey"] is IProjectViewModel, "Invalid project");

            Project = (IProjectViewModel)workflow.Data["projectKey"];
			return true;
		}
	}
}
