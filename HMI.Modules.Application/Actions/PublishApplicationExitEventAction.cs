// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using HMI.Infrastructure.Commanding;
using HMI.Modules.Application.Events;
namespace HMI.Modules.Application.Actions
{
	public class PublishApplicationExitEventAction : IWorkflowAction
	{
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public IEventAggregator EventAggregator
		{
			get;
			private set;
		}
		public PublishApplicationExitEventAction(IEventAggregator eventAggregator)
		{
            EventAggregator = eventAggregator;
		}
		public bool Execute(IWorkflow workflow)
		{
			ApplicationExitEvent @event = EventAggregator.GetEvent<ApplicationExitEvent>();
			if (@event != null)
			{
				@event.Publish(null);
			}
			return true;
		}
	}
}
