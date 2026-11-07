// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;

namespace HMI.Modules.Artboard.Commandhandlers
{
	public class MakeSameHeightHandler : GroupOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.MakeSameHeight;
			}
		}
		public MakeSameHeightHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("MakeSameHeight");
			actionNames.Add("RecalculateSelectionBounds");
			actionNames.Add("EndUndoGroup");
		}
	}
}
