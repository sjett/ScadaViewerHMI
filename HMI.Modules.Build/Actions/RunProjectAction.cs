// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Modules.Build.Services.Builder;
namespace HMI.Modules.Build.Actions
{
	internal class RunProjectAction : IWorkflowAction
	{
		private readonly Lazy<IProjectRunnerService> projectRunnerService;
		private readonly IServiceProvider serviceLocator;
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Asynchronous;
			}
		}
		public RunProjectAction(IServiceProvider serviceLocator)
		{
			this.serviceLocator = serviceLocator;
            projectRunnerService = new Lazy<IProjectRunnerService>(serviceLocator.Resolve<Func<IProjectRunnerService>>());
		}
		public bool Execute(IWorkflow workflow)
		{
            projectRunnerService.Value.Run();
			return true;
		}
	}
}
