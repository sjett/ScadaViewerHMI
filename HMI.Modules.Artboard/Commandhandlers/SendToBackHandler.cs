// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure;

namespace HMI.Modules.Artboard.Commandhandlers
{
	public class SendToBackHandler : ZOperationHandler
	{
		protected override ICommand Command
		{
			get
			{
				return DesignerCanvasCommands.SendToBack;
			}
		}
		public SendToBackHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("SendToBack");
			actionNames.Add("EndUndoGroup");
		}
	}
}
