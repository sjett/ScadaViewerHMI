// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public class RequestUserInteractionDetails : InteractionDetails
	{
		
		public string Title
		{
			get;
			private set;
		}
		public string Text
		{
			get;
			private set;
		}
		public IList<UserAction> Actions
		{
			get;
			private set;
		}
		public RequestUserInteractionDetails(string title, string text, params UserAction[] actions)
		{
            Title = title;
            Text = text;
            Actions = new List<UserAction>(actions);
		}
	}
}
