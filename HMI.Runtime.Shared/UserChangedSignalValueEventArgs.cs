// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public class UserChangedSignalValueEventArgs : EventArgs
	{
		private string signalName;
		private object signalValue;
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
		}
		public object SignalValue
		{
			get
			{
				return this.signalValue;
			}
		}
		public UserChangedSignalValueEventArgs(string signalName, object signalValue)
		{
			this.signalName = signalName;
			this.signalValue = signalValue;
		}
	}
}
