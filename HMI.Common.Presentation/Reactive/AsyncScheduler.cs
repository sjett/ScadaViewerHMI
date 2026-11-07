// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Reactive.Concurrency;
namespace HMI.Common.Presentation.Reactive
{
	public class AsyncScheduler : IAsyncScheduler
	{
		private readonly ThreadPoolScheduler _threadPoolScheduler = ThreadPoolScheduler.Instance;//  Scheduler.ThreadPool;
		public DateTimeOffset Now
		{
			get
			{
				return _threadPoolScheduler.Now;

            }
		}
		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			return _threadPoolScheduler.Schedule<TState>(state, action);
		}
		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			return _threadPoolScheduler.Schedule<TState>(state, dueTime, action);
		}
		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			return _threadPoolScheduler.Schedule<TState>(state, dueTime, action);
		}
	}
}
