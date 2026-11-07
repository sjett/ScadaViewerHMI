// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Threading;
using System.Windows.Threading;
namespace HMI.Runtime.Core.Helper
{
    public interface ICoreTimer : IDisposable
    {
        event EventHandler Tick;
        TimeSpan Interval
        {
            get;
            set;
        }
        double SecondInterval
        {
            get;
            set;
        }
        bool IsEnabled
        {
            get;
        }
        void Start();
        void Stop();
    }

    public class CoreTimer : IDisposable, ICoreTimer
	{
		private DispatcherTimer _dispatcherTimer;

        public event EventHandler Tick;

		public TimeSpan Interval
		{
			get
			{
				return _dispatcherTimer.Interval;
			}
			set
			{
                _dispatcherTimer.Interval=value;
			}
		}
		public double SecondInterval
		{
			get
			{
				return _dispatcherTimer.Interval.TotalSeconds;
			}
			set
			{
                _dispatcherTimer.Interval=TimeSpan.FromSeconds(value);
			}
		}
		public bool IsEnabled
		{
			get
			{
				return _dispatcherTimer.IsEnabled;
			}
		}
		public CoreTimer()
		{
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += OnDispatcherTimerTick;
		}
		private void OnDispatcherTimerTick(object sender, EventArgs e)
		{
            if (Tick != null)
			{
                Tick(sender, e);
			}
		}
		public void Start()
		{
            _dispatcherTimer.Start();
		}
		public void Stop()
		{
            _dispatcherTimer.Stop();
		}
		public void Dispose()
		{
			if (_dispatcherTimer == null)
			{
				return;
			}
            _dispatcherTimer.Stop();
            _dispatcherTimer.Tick -= OnDispatcherTimerTick;
            _dispatcherTimer = null;
		}
	}
}
