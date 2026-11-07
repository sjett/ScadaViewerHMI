// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;
namespace HMI.Modules.Application.Actions
{
	public abstract class ShowBusyIndicatorAction : IWorkflowAction
	{
		private readonly Lazy<INotificationService> _notificationService;
		protected abstract string Message
		{
			get;
		}
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public ShowBusyIndicatorAction(IServiceProvider serviceLocator)
		{
            _notificationService = new Lazy<INotificationService>(serviceLocator.Resolve<Func<INotificationService>>());
		}
		public virtual bool Execute(IWorkflow workflow)
		{
            _notificationService.Value.ShowBusyIndicator(Message);
			return true;
		}
	}
}
