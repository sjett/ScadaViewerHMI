// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Modules.Build.Services.Builder;
namespace HMI.Modules.Build.Actions
{
	public class BuildProjectAction : IWorkflowAction
	{
		private readonly Lazy<IProjectBuilderService> projectBuilderService;
		private readonly IServiceProvider serviceLocator;
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Asynchronous;
			}
		}
		public BuildProjectAction(IServiceProvider serviceLocator)
		{
			this.serviceLocator = serviceLocator;
            projectBuilderService = new Lazy<IProjectBuilderService>(serviceLocator.Resolve<Func<IProjectBuilderService>>());
		}
		public bool Execute(IWorkflow workflow)
		{
			IProjectBuilderService value = projectBuilderService.Value;
			if (!value.Prepare(workflow.SynchronizationContext) || !value.Build() || !value.Publish())
			{
                InsertBuildFailedNotification(workflow);
                InsertHideBusyIndicator(workflow);
			}
			return true;
		}
		private void InsertHideBusyIndicator(IWorkflow workflow)
		{
			IWorkflowAction instance = serviceLocator.Resolve<IWorkflowAction>("HideBuildingProjectBusyIndicator");
			workflow.InsertAction(instance);
		}
		private void InsertBuildFailedNotification(IWorkflow workflow)
		{
			IWorkflowAction instance = serviceLocator.Resolve<IWorkflowAction>("BuildFailedNotification");
			workflow.InsertAction(instance);
		}
	}
}
