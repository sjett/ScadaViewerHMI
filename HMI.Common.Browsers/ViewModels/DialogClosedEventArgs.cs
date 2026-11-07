// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Browsers.ViewModels
{
	public class DialogClosedEventArgs : EventArgs
	{
		
		public bool? DialogResult
		{
			get;
			private set;
		}
		public DialogClosedEventArgs(bool? dialogResult)
		{
            DialogResult = dialogResult;
		}
	}
}
