// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Modules.Artboard.Services;
namespace HMI.Modules.Artboard.Actions
{
	public abstract class GroupOperationAction : IWorkflowAction
	{
		protected Lazy<IProjectManagerViewModel> Solution
		{
			get;
			private set;
		}
		protected Lazy<IControlLayoutService> ControlLayoutService
		{
			get;
			private set;
		}
		protected Lazy<IZOrderService> ZOrderService
		{
			get;
			private set;
		}
		public virtual ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public GroupOperationAction(IServiceProvider serviceLocator)
		{
            Solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
            ControlLayoutService = new Lazy<IControlLayoutService>(serviceLocator.Resolve<Func<IControlLayoutService>>());
            ZOrderService = new Lazy<IZOrderService>(serviceLocator.Resolve<Func<IZOrderService>>());
		}
		public abstract bool Execute(IWorkflow workflow);
	}
}
