// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public abstract class GroupOperationHandler : WorkflowCommandHandler
	{
		protected readonly Lazy<IProjectManagerViewModel> solution;
		public GroupOperationHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			IProjectPageViewModel activePage = solution.Value.ActivePage;
			return activePage != null && IsSelectionValidGroup(activePage);
		}
		private bool IsSelectionValidGroup(IProjectPageViewModel page)
		{
			ICollection<ICommonControlViewModel> selectedItems = page.GetSelectedItems();
			return selectedItems.Count >= 2 && AreAllSelectedItemsOnSamePage(page, selectedItems);
		}
		private bool AreAllSelectedItemsOnSamePage(IProjectPageViewModel page, ICollection<ICommonControlViewModel> selectedItems)
		{
			return selectedItems.All((i) => i.Parent == page);
		}
	}
}
