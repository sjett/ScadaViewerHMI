// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using System.Threading;
namespace HMI.Runtime.Core.Helper
{
	public class CoreTimerDesign : IDisposable, ICoreTimer
	{


        public event EventHandler Tick;

		public TimeSpan Interval
		{
			get;
			set;
		}
		public double SecondInterval
		{
			get;
			set;
		}
		public bool IsEnabled
		{
			get
			{
				return false;
			}
		}
		public void Start()
		{
		}
		public void Stop()
		{
		}
		public void Dispose()
		{
		}
	}
}
