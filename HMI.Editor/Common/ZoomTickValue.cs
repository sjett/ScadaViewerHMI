// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Common
{
	public class ZoomTickValue
	{
		public double Tick
		{
			get;
			set;
		}
		public double Value
		{
			get;
			set;
		}
		public ZoomTickValue()
		{
		}
		public ZoomTickValue(double tick, double value)
		{
            Tick = tick;
            Value = value;
		}
	}
}
