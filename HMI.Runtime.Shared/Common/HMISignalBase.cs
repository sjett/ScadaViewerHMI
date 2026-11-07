// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared.Common
{
	public abstract class HMISignalBase : PropertyChangedNotifier
	{
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
		}
		public HMISignalBase(string signalName)
		{
            name = signalName;
		}
	}
}
