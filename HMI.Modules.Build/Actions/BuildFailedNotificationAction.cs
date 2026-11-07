// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;
using HMI.Modules.Build.Resources;
namespace HMI.Modules.Build.Actions
{
	public class BuildFailedNotificationAction : IWorkflowAction
	{
		private readonly Lazy<INotificationService> notificationService;
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public BuildFailedNotificationAction(IServiceProvider serviceLocator)
		{
            notificationService = new Lazy<INotificationService>(serviceLocator.Resolve<Func<INotificationService>>());
		}
		public bool Execute(IWorkflow workflow)
		{
            notificationService.Value.ShowErrorDialog(BuildErrorTexts.BuildFailedError, null);
			return false;
		}
	}
}
