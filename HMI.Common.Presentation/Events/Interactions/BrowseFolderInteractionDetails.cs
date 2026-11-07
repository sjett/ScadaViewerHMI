// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public sealed class BrowseFolderInteractionDetails : InteractionDetails
	{
		public BrowseFolderInteractionCallback Callback
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public Environment.SpecialFolder RootFolder
		{
			get;
			set;
		}
		public string SelectedPath
		{
			get;
			set;
		}
		public bool IncludeNetworkFoldersBelowDomainLevel
		{
			get;
			set;
		}
		public bool ShowBothFilesAndFolders
		{
			get;
			set;
		}
		public bool ShowEditBox
		{
			get;
			set;
		}
		public bool ShowFullPathInEditBox
		{
			get;
			set;
		}
		public bool ShowNewFolderButton
		{
			get;
			set;
		}
		public object State
		{
			get;
			set;
		}
		public BrowseFolderInteractionDetails(string selectedPath, BrowseFolderInteractionCallback callback, object state)
		{
            SelectedPath = selectedPath;
            Callback = callback;
            State = state;
            ShowEditBox = true;
            ShowFullPathInEditBox = true;
            ShowNewFolderButton = true;
		}
	}
}
