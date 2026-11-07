// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
namespace HMI.Runtime.Shared
{
	public class ConditionSignalItem
	{
		private string parameterName;
		private string signalName;

		[Category("HMI")]
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			set
			{
				this.parameterName = value;
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
			set
			{
				this.signalName = value;
			}
		}
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.parameterName, this.signalName);
		}
	}
}
