// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public abstract class ZOperationHandler : WorkflowCommandHandler
	{
		protected readonly Lazy<IProjectManagerViewModel> ProjectManager;
		public ZOperationHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            ProjectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			IProjectPageViewModel activePage = ProjectManager.Value.ActivePage;
			return activePage != null && HasEligibleSelection(activePage);
		}
        private bool HasEligibleSelection(IProjectPageViewModel page)
        {
            ICollection<ICommonControlViewModel> selectedItems = page.GetSelectedItems();
            return selectedItems.Count >= 1 && AreAllSelectedItemsOnPage(page, selectedItems);
        }
        private bool AreAllSelectedItemsOnPage(IProjectPageViewModel page, ICollection<ICommonControlViewModel> selectedItems)
        {
            return selectedItems.All(item => item.Parent == page);
        }
    }
}
