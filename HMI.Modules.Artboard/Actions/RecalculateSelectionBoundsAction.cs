// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class RecalculateSelectionBoundsAction : IWorkflowAction
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public RecalculateSelectionBoundsAction(IServiceProvider serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		public bool Execute(IWorkflow workflow)
		{
            _projectManager.Value.ActivePage.RecalculateSelectionBounds();
			return true;
		}
	}
}
