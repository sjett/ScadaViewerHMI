// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class DeleteAction : ClipboardAction
	{
		private readonly Lazy<IProjectManagerService> _projectManager;
		public DeleteAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerService>(new Func<IProjectManagerService>(serviceLocator.Resolve<IProjectManagerService>));
		}
		public override bool Execute(IWorkflow workflow)
		{
			ICollection<ICommonControlViewModel> selectedItems = ActivePage.GetSelectedItems();
            _projectManager.Value.RemoveItems(ActivePage, selectedItems);
			return true;
		}
	}
}
