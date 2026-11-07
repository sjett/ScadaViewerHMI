// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
namespace HMI.Runtime.Shared.Common
{
	public class PropertyChangedNotifier : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public void RaisePropertyChanged<T>(Expression<Func<T>> action)
		{
            PropertyChanged?.Raise(this, action);
		}
		public void RaisePropertyChanged<T>(object sender, Expression<Func<T>> action)
		{
            PropertyChanged?.Raise(sender, action);
		}
		protected void MonitorNotifyProperty<T>(Expression<Func<T>> property) where T : INotifyPropertyChanged
		{
            MonitorNotifyProperty(property, delegate(object sender, PropertyChangedEventArgs e)
			{
                RaisePropertyChanged(property);
			});
		}
		protected void MonitorNotifyProperty<T>(Expression<Func<T>> property, PropertyChangedEventHandler raisePropertyChangedAction) where T : INotifyPropertyChanged
		{
			Func<T> func = property.Compile();
			T t = func.Invoke();
			if (t != null)
			{
				t.PropertyChanged += raisePropertyChangedAction;
			}
		}
		protected void UnMonitorNotifyProperty<T>(Expression<Func<T>> property) where T : INotifyPropertyChanged
		{
            UnMonitorNotifyProperty(property, delegate(object sender, PropertyChangedEventArgs e)
			{
                RaisePropertyChanged(property);
			});
		}
		protected void UnMonitorNotifyProperty<T>(Expression<Func<T>> property, PropertyChangedEventHandler raisePropertyChangedAction) where T : INotifyPropertyChanged
		{
			Func<T> func = property.Compile();
			T t = func.Invoke();
			if (t != null)
			{
				t.PropertyChanged-=raisePropertyChangedAction;
			}
		}
		protected void MonitorObservableCollection<T>(Expression<Func<ObservableCollection<T>>> property)
		{
            MonitorObservableCollection(property, delegate(object sender, NotifyCollectionChangedEventArgs e)
			{
                RaisePropertyChanged(property);
			});
		}
		protected void MonitorObservableCollection<T>(Expression<Func<ObservableCollection<T>>> property, NotifyCollectionChangedEventHandler notifyCollectionChangedAction)
		{
			Func<ObservableCollection<T>> func = property.Compile();
			ObservableCollection<T> observableCollection = func.Invoke();
			if (observableCollection != null)
			{
				observableCollection.CollectionChanged+=notifyCollectionChangedAction;
			}
		}
		protected void UnMonitorObservableCollection<T>(Expression<Func<ObservableCollection<T>>> property)
		{
            UnMonitorObservableCollection(property, delegate(object sender, NotifyCollectionChangedEventArgs e)
			{
                RaisePropertyChanged(property);
			});
		}
		protected void UnMonitorObservableCollection<T>(Expression<Func<ObservableCollection<T>>> property, NotifyCollectionChangedEventHandler notifyCollectionChangedAction)
		{
			Func<ObservableCollection<T>> func = property.Compile();
			ObservableCollection<T> observableCollection = func.Invoke();
			if (observableCollection != null)
			{
				observableCollection.CollectionChanged-=notifyCollectionChangedAction;
			}
		}
		[Obsolete("Use RaisePropertyChanged<T> instead")]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		[Obsolete("Use RaisePropertyChanged<T> instead")]
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
