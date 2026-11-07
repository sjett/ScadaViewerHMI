// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class SendToBackAction : GroupOperationAction
	{
		public SendToBackAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			IProjectPageViewModel activePage = Solution.Value.ActivePage;
			ICollection<ICommonControlViewModel> selectedItems = activePage.GetSelectedItems();
			IExplorerNodeViewModel arg_28_0 = Solution.Value.SelectedItem;
            ZOrderService.Value.SendToBack(activePage, selectedItems);
			return true;
		}
	}
}
