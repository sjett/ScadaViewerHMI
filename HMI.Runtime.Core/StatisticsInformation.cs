// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
namespace HMI.Runtime.Core
{
    public interface IStatisticsInformation : INotifyPropertyChanged
    {
        string ServerName
        {
            get;
            set;
        }
        int NumberOfCalls
        {
            get;
            set;
        }
        InformationState State
        {
            get;
            set;
        }
    }

    public class StatisticsInformation : INotifyPropertyChanged, IStatisticsInformation
	{
		private int _numberOfCalls;
		private InformationState _state = InformationState.Unchanged;
		private string _serverName;
		public event PropertyChangedEventHandler PropertyChanged;
		
		public string ServerName
		{
			get
			{
				return _serverName;
			}
			set
			{
                _serverName = value;
                OnPropertyChanged(() => ServerName);
			}
		}
		public int NumberOfCalls
		{
			get
			{
				return _numberOfCalls;
			}
			set
			{
                UpdateCallCountState(_numberOfCalls, value);
                _numberOfCalls = value;
                OnPropertyChanged(() => NumberOfCalls);
			}
		}
		public InformationState State
		{
			get
			{
				return _state;
			}
			set
			{
                _state = value;
                OnPropertyChanged(() => State);
			}
		}
		private void UpdateCallCountState(int oldValue, int newValue)
		{
			if (oldValue == newValue)
			{
                State = InformationState.Unchanged;
				return;
			}
			if (oldValue > newValue)
			{
                State = InformationState.Decreased;
				return;
			}
            State = InformationState.Increased;
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
	}
}
