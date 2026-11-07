// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.RuntimeControls.General.UserLoginDialogs
{
	public class DialogResource
	{
		public string Name
		{
			get;
			set;
		}
		public string SymbolicText
		{
			get;
			set;
		}
		public string DefaultText
		{
			get;
			set;
		}
		public string Translation
		{
			get;
			set;
		}
		public DialogResource(string resourceName, string resourceSymbolicText, string defaultText = null)
		{
			this.Name = resourceName;
			this.SymbolicText = resourceSymbolicText;
			this.DefaultText = defaultText;
			this.Translation = this.DefaultText;
		}
	}
}
