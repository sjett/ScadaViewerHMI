// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Threading;

namespace HMI.Infrastructure
{
    /// <summary>
    /// 提供一个延迟执行的Action，调用Execute方法后会在指定的时间间隔后执行Action，如果在时间间隔内再次调用Execute方法，则会重新计时。
    /// </summary>
    public class LazyAction : IDisposable
    {
        private DispatcherTimer _dispatcherTimer;
        private readonly TimeSpan _delay;
        private readonly Action _action;
        private bool _isDisposed;
        public LazyAction(TimeSpan deferedUpdateDelay, Action action)
        {
            _action = action;
            _delay = deferedUpdateDelay;
        }
        public void Execute()
        {
            StopTimer();
            InitializeTimer(_delay);
            _dispatcherTimer.Start();
        }
        private void InitializeTimer(TimeSpan interval)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = interval;
            _dispatcherTimer.Tick += OnTimerTick;
        }
        private void StopTimer()
        {
            if (_dispatcherTimer == null)
            {
                return;
            }
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= OnTimerTick;
            _dispatcherTimer = null;
        }
        private void OnTimerTick(object? sender, EventArgs e)
        {
            _action();
            _dispatcherTimer.Stop();
        }
        protected virtual void DisposeManagedResources()
        {
            StopTimer();
        }
        protected virtual void DisposeUnmanagedResources()
        {
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }
                DisposeUnmanagedResources();
            }
            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~LazyAction()
        {
            Dispose(false);
        }
    }
}
