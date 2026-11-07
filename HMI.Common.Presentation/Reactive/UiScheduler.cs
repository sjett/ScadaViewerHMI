// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Reactive.Concurrency;
using System.Threading;
namespace HMI.Common.Presentation.Reactive
{
	public class UiScheduler : SynchronizationContextScheduler, IUiScheduler, IScheduler
	{
		public UiScheduler(SynchronizationContext context) : base(context)
		{
		}
	}
}
