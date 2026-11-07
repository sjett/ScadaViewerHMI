// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class AlignRightHandler : GroupOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.AlignRight;
			}
		}
		public AlignRightHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("AlignRight");
			actionNames.Add("RecalculateSelectionBounds");
			actionNames.Add("EndUndoGroup");
		}
	}
}
