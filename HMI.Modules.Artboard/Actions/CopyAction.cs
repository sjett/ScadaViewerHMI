// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Modules.Artboard.Actions
{
	public class CopyAction : ClipboardAction
	{
		public CopyAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			ICollection<ICommonControlViewModel> selectedItems = ActivePage.GetSelectedItems();
            ClipboardService.SetData(selectedItems);
			return true;
		}
	}
}
