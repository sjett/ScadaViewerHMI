// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public class BusyOperationDetails
	{
		public bool IsBusy
		{
			get;
			set;
		}
		public string Message
		{
			get;
			set;
		}
		public BusyOperationDetails(bool isBusy, string message = null)
		{
            IsBusy = isBusy;
            Message = message;
		}
		public BusyOperationDetails()
		{
		}
	}
}
