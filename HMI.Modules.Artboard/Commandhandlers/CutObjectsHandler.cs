// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class CutObjectsHandler : WorkflowCommandHandler
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		protected override ICommand Command
		{
			get
			{
				return ApplicationCommands.Cut;
			}
		}
		public CutObjectsHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			IProjectPageViewModel activePage = _projectManager.Value.ActivePage;
			return activePage != null && activePage.GetSelectedItems().Count != 0 && !activePage.GetSelectedItems().OfType<INavigationTargetViewModel>().Any<INavigationTargetViewModel>();
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("Copy");
			actionNames.Add("Delete");
			actionNames.Add("EndUndoGroup");
		}
	}
}
