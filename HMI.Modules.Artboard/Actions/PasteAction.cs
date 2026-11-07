// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public class PasteAction : ClipboardAction
	{
		
		private static Func<IExplorerNodeViewModel, bool> _shouldDeselectNode;
		public PasteAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			ICollection<ICommonControlViewModel> data = ClipboardService.GetData(ActivePage);
			IProjectManagerViewModel projectManager = Solution;
			if (_shouldDeselectNode == null)
			{
                _shouldDeselectNode = new Func<IExplorerNodeViewModel, bool>(ShouldDeselectNode);
			}
			projectManager.Deselect(_shouldDeselectNode);
            ActivePage.AddTopmostChildren(data, true);
            ActivePage.SelectItems(data);
			return true;
		}
		
		private static bool ShouldDeselectNode(IExplorerNodeViewModel node)
		{
			return true;
		}
	}
}
