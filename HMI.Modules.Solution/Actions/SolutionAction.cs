// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;

namespace HMI.Modules.Solution.Actions
{
	public abstract class SolutionAction : IWorkflowAction
	{
		protected Lazy<IProjectManagerService> ProjectManagerService
		{
			get;
			private set;
		}
		protected Lazy<INotificationService> NotificationService
		{
			get;
			private set;
		}
		protected Lazy<IProjectManagerViewModel> Solution
		{
			get;
			private set;
		}
		protected IServiceProvider ServiceLocator
		{
			get;
			private set;
		}
		protected IProjectViewModel Project
		{
			get
			{
				return Solution.Value.Project;
			}
			set
			{
                Solution.Value.Project = value;
			}
		}
		public virtual ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		protected SolutionAction(IServiceProvider serviceLocator)
		{
            ServiceLocator = serviceLocator;
            Solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
            NotificationService = new Lazy<INotificationService>(serviceLocator.Resolve<Func<INotificationService>>());
            ProjectManagerService = new Lazy<IProjectManagerService>(serviceLocator.Resolve<Func<IProjectManagerService>>());
		}
		public abstract bool Execute(IWorkflow workflow);
		protected void InsertWorkflowAction(string actionName, IWorkflow workflow)
		{
			IWorkflowAction workflowAction = GetWorkflowAction(actionName);
			workflow.InsertAction(workflowAction);
		}
		private IWorkflowAction GetWorkflowAction(string actionName)
		{
			return ServiceLocator.Resolve<IWorkflowAction>(actionName);
		}
	}
}
