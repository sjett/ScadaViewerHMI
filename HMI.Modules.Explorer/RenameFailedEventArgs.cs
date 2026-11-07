// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using HMI.Common.Instrumentation.Events;
namespace HMI.Modules.Explorer
{
	public class RenameFailedEventArgs : ValueChangedEventArgs<string>
	{
		
		public string Message
		{
			get;
			private set;
		}
		public RenameFailedEventArgs(string oldName, string newName, string message) : base(oldName, newName)
		{
            Message = message;
		}
	}
}
