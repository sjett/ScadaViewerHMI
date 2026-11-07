// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;

namespace HMI.Infrastructure
{
    public class CancelParameterEventArgs<T> : ParameterEventArgs<T>
	{
		public bool CancelEvent
		{
			get;
			set;
		}
		public CancelParameterEventArgs(T value) : base(value)
		{
		}
	}
}
