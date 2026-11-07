// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;

namespace HMI.Modules.Artboard.Commandhandlers
{
	public class BringToFrontHandler : ZOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.BringToFront;
			}
		}
		public BringToFrontHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("BringToFront");
			actionNames.Add("EndUndoGroup");
		}
	}
}
