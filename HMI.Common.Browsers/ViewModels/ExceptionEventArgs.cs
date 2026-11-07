// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Browsers.ViewModels
{
	public class ExceptionEventArgs : EventArgs
	{
		
		public Exception Exception
		{
			get;
			private set;
		}
		public ExceptionEventArgs(Exception exception)
		{
            Exception = exception;
		}
	}
}
