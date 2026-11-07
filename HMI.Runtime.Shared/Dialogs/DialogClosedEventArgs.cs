// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared.Dialogs
{
	public class DialogClosedEventArgs : EventArgs
	{
		private DialogResult dialogResult;
		public DialogResult DialogResult
		{
			get
			{
				return dialogResult;
			}
		}
		public DialogClosedEventArgs(DialogResult dialogResult)
		{
			this.dialogResult = dialogResult;
		}
	}
}
