// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class BringForwardHandler : ZOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.BringForward;
			}
		}
		public BringForwardHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("BringForward");
			actionNames.Add("EndUndoGroup");
		}
	}
}
