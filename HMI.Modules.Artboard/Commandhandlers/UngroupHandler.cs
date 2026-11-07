// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class UngroupHandler : WorkflowCommandHandler
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.Ungroup;
			}
		}
		public UngroupHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			IProjectPageViewModel activePage = _projectManager.Value.ActivePage;
			return activePage != null && activePage.GetSelectedItems().Count == 1 && activePage.SelectedItem is IControlGroupContainerViewModel;
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("Ungroup");
			actionNames.Add("EndUndoGroup");
		}
	}
}
