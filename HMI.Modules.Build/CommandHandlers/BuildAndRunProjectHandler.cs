// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Build.CommandHandlers
{
	internal class BuildAndRunProjectHandler : WorkflowCommandHandler
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		protected override ICommand Command
		{
			get
			{
				return EditorCommands.Build;
			}
		}
		public BuildAndRunProjectHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			return _projectManager.Value.Project != null;
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("ShowBuildingProjectBusyIndicator");
			actionNames.Add("Save");
			actionNames.Add("BuildProject");
			actionNames.Add("RunProject");
			actionNames.Add("HideBuildingProjectBusyIndicator");
		}
	}
}
