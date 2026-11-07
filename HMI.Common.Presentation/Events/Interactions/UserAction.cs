// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public class UserAction
	{
		public string ActionText
		{
			get;
			private set;
		}
		public UserInteractionHandler ActionCallback
		{
			get;
			private set;
		}
		public object State
		{
			get;
			set;
		}
		public UserAction(string actionText, UserInteractionHandler actionCallback, object state)
		{
            State = state;
            ActionCallback = actionCallback;
            ActionText = actionText;
		}
	}
}
