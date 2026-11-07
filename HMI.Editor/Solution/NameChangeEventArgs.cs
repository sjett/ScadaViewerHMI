// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Solution
{
	public class NameChangeEventArgs : EventArgs
    {
		public string OldName
		{
			get;
			set;
		}
		public string NewName
		{
			get;
			set;
		}
		public NameChangeEventArgs(string oldName, string newName)
		{
            OldName = oldName;
            NewName = newName;
		}
	}
}
