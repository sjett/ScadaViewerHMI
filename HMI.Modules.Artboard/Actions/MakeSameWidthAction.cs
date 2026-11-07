// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class MakeSameWidthAction : GroupOperationAction
	{
		public MakeSameWidthAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			IProjectPageViewModel activePage = Solution.Value.ActivePage;
			ICollection<ICommonControlViewModel> selectedItems = activePage.GetSelectedItems();
			ICommonControlViewModel firstItem = Solution.Value.SelectedItem as ICommonControlViewModel;
            ControlLayoutService.Value.MakeSameWidth(firstItem, selectedItems);
			return true;
		}
	}
}
