// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public sealed class SaveFileInteractionDetails : FileInteractionDetails
	{
		public SaveFileInteractionCallback Callback
		{
			get;
			set;
		}
		public bool CreatePrompt
		{
			get;
			set;
		}
		public string FileName
		{
			get;
			set;
		}
		public bool OverwritePrompt
		{
			get;
			set;
		}
		public bool ValidateNames
		{
			get;
			set;
		}
		public SaveFileInteractionDetails(string filter, SaveFileInteractionCallback callback, object state) : base(filter, state)
		{
            Callback = callback;
            CreatePrompt = true;
            OverwritePrompt = true;
            ValidateNames = true;
		}
	}
}
