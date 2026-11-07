// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;

namespace HMI.Infrastructure
{
    public class ParameterEventArgs<T> : EventArgs
	{
		public T Parameter
		{
			get;
			private set;
		}
		public ParameterEventArgs(T value)
		{
            Parameter = value;
		}
	}
}
