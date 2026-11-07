// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public sealed class OpenFileInteractionDetails : FileInteractionDetails
	{
		
		public OpenFileInteractionCallback Callback
		{
			get;
			set;
		}
		public bool Multiselect
		{
			get;
			set;
		}
		public bool ShowReadOnly
		{
			get;
			set;
		}
		public bool ReadOnlyChecked
		{
			get;
			set;
		}
		public OpenFileInteractionDetails(string filter, OpenFileInteractionCallback callback, object state) : base(filter, state)
		{
            Callback = callback;
            Multiselect = false;
            ShowReadOnly = false;
            ReadOnlyChecked = false;
		}
	}
}
