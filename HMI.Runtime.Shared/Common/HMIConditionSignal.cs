// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared.Common
{
	public class HMIConditionSignal : HMINumericSignal
	{
		private string parameterName;
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
		}
		public HMIConditionSignal(string signalName, string parameterName) : base(signalName)
		{
			this.parameterName = parameterName;
		}
	}
}
