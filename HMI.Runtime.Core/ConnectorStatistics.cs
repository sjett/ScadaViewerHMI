// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Core.Helper.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HMI.Runtime.Core
{
    public interface IConnectorStatistics : INotifyPropertyChanged, IDisposable
    {
        ObservableCollection<IStatisticsInformation> GetUpdatesPerSecond
        {
            get;
        }
        void IncrementGetUpdatesCount(string serverName);
    }

    /// <summary>
    /// 统计各服务器每秒获取信号更新的次数。
    /// </summary>
    public class ConnectorStatistics : IConnectorStatistics
	{
		private Dictionary<string, int> updateCountsByServer = new Dictionary<string, int>();
		private ICoreTimer timer;
		private bool isDisposed;
		
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<IStatisticsInformation> GetUpdatesPerSecond
		{
			get;
			private set;
		}
		public ConnectorStatistics(ICoreTimer timer)
		{
            updateCountsByServer = new Dictionary<string, int>();
            GetUpdatesPerSecond = new ObservableCollection<IStatisticsInformation>();
            if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}
            StartTimer(timer);
		}
		public void IncrementGetUpdatesCount(string serverName)
		{
			string text = string.IsNullOrWhiteSpace(serverName) ? "." : serverName;
			if (updateCountsByServer.ContainsKey(text))
			{
                updateCountsByServer[text] = updateCountsByServer[text] + 1;
				return;
			}
            updateCountsByServer.Add(text, 1);
		}
		private void OnTimerTick(object sender, EventArgs e)
		{
            RefreshUpdateCounts();
            ResetUpdateCounts();
		}
		private void StartTimer(ICoreTimer timer)
		{
            this.timer = timer;
            this.timer.SecondInterval = 1.0;
            this.timer.Tick += new EventHandler(OnTimerTick);
            this.timer.Start();
		}
		private void RefreshUpdateCounts()
		{
			IEnumerable<IStatisticsInformation> existingStatistics = GetUpdatesPerSecond.Where(HasUpdateCount);
			IEnumerable<string> newServerNames = updateCountsByServer.Keys.Where(DoesNotHaveStatisticsEntry);
			using (IEnumerator<IStatisticsInformation> enumerator = existingStatistics.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IStatisticsInformation statistics = enumerator.Current;
					statistics.NumberOfCalls = updateCountsByServer[statistics.ServerName];
				}
			}
			using (IEnumerator<string> newServerNameEnumerator = newServerNames.GetEnumerator())
			{
				while (newServerNameEnumerator.MoveNext())
				{
					string serverName = newServerNameEnumerator.Current;
                    GetUpdatesPerSecond.Add(new StatisticsInformation
					{
						ServerName = serverName,
						NumberOfCalls = updateCountsByServer[serverName]
					});
				}
			}
		}
		private void ResetUpdateCounts()
		{
			string[] serverNames = updateCountsByServer.Keys.ToArray();
			for (int i = 0; i < serverNames.Length; i++)
			{
				string serverName = serverNames[i];
                updateCountsByServer[serverName] = 0;
			}
		}
		protected virtual void OnPropertyChanged(string propertyName)
		{
            if (PropertyChanged != null)
			{
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		protected virtual void OnPropertyChanged<T>(Expression<Func<T>> action)
		{
            OnPropertyChanged(GetPropertyName(action));
		}
		protected static string GetPropertyName<T>(Expression<Func<T>> action)
		{
			MemberExpression memberExpression = (MemberExpression)action.Body;
			return memberExpression.Member.Name;
		}
		protected virtual void DisposeManagedResources()
		{
		}
		protected virtual void DisposeUnmanagedResources()
		{
			if (timer != null)
			{
                timer.Stop();
                timer.Tick -= new EventHandler(OnTimerTick);
                timer.Dispose();
                timer = null;
			}
            GetUpdatesPerSecond.Clear();
            updateCountsByServer.Clear();
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
                    DisposeManagedResources();
				}
                DisposeUnmanagedResources();
			}
            isDisposed = true;
		}
		public void Dispose()
		{
            Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		private bool HasUpdateCount(IStatisticsInformation statistics)
		{
			return updateCountsByServer.ContainsKey(statistics.ServerName);
		}
		
		private bool DoesNotHaveStatisticsEntry(string serverName)
		{
            return !GetUpdatesPerSecond.Any(statistics => statistics.ServerName == serverName);
		}
	}
}
