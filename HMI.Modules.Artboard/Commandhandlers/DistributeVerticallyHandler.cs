// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;

namespace HMI.Modules.Artboard.Commandhandlers
{
	public class DistributeVerticallyHandler : GroupOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.DistributeVertical;
			}
		}
		public DistributeVerticallyHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("DistributeVertically");
			actionNames.Add("RecalculateSelectionBounds");
			actionNames.Add("EndUndoGroup");
		}
	}
}
