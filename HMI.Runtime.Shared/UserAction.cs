// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
    public delegate void UserInteractionHandler(object state);

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
			this.State = state;
			this.ActionCallback = actionCallback;
			this.ActionText = actionText;
		}
		public virtual object GetActionContent()
		{
			return this.ActionText;
		}
	}
}
