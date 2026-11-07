// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class SendBackwardHandler : ZOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.SendBackward;
			}
		}
		public SendBackwardHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("SendBackward");
			actionNames.Add("EndUndoGroup");
		}
	}
}
