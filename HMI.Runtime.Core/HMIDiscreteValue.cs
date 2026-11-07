// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
	public class HMIDiscreteValue : HMIEntity
	{
		public Guid DiscreteTypeID
		{
			get;
			set;
		}
		public double Value
		{
			get;
			set;
		}
	}
}
