// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.RuntimeControls.Gauges
{
	public interface ICalculateAngle
	{
		bool IsSinusoidal
		{
			set;
		}
		double MaximumAngle
		{
			set;
		}
		double MaximumValue
		{
			set;
		}
		double MinimunAngle
		{
			set;
		}
		double MinimumValue
		{
			set;
		}
		double GetAngleValue(double signalValue);
	}
}
