// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared.Common
{
	public class HMINumericSignal : HMISignalBase
	{
		private double value;
		public double Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
                RaisePropertyChanged(() => Value);
			}
		}
		public HMINumericSignal(string signalName) : this(signalName, double.NaN)
		{
		}
		public HMINumericSignal(string signalName, double value) : base(signalName)
		{
            Value = value;
		}
		public static double ConvertSignalValueToDouble(object signalValue)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(signalValue.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
	}
}
