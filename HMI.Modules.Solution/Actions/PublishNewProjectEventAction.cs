// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Events;
namespace HMI.Modules.Solution.Actions
{
	public class PublishNewProjectEventAction : IWorkflowAction
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
		public PublishNewProjectEventAction(IEventAggregator eventAggregator)
		{
            EventAggregator = eventAggregator;
		}
		public bool Execute(IWorkflow workflow)
		{
			NewProjectEvent @event = EventAggregator.GetEvent<NewProjectEvent>();
			if (@event != null)
			{
				@event.Publish(null);
			}
			return true;
		}
	}
}
