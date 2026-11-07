// SPDX-License-Identifier: GPL-3.0-only
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace HMI.Common.Presentation.Events.Interactions
{
	public abstract class FileInteractionDetails : InteractionDetails
	{
		
		public string Filter
		{
			get;
			set;
		}
		public int FilterIndex
		{
			get;
			set;
		}
		public object State
		{
			get;
			set;
		}
		public string Title
		{
			get;
			set;
		}
		public string InitialDirectory
		{
			get;
			set;
		}
		public bool AddExtension
		{
			get;
			set;
		}
		public bool CheckFileExists
		{
			get;
			set;
		}
		public bool CheckPathExists
		{
			get;
			set;
		}
		public string DefaultExt
		{
			get;
			set;
		}
		public bool DereferenceLinks
		{
			get;
			set;
		}
		public ICollection<FileDialogCustomPlace> CustomPlaces
		{
			get;
			private set;
		}
		protected FileInteractionDetails(string filter, object state)
		{
            Filter = filter;
            FilterIndex = 1;
            State = state;
            AddExtension = true;
            CheckFileExists = true;
            CheckPathExists = true;
            CustomPlaces = new Collection<FileDialogCustomPlace>();
		}
	}
}
