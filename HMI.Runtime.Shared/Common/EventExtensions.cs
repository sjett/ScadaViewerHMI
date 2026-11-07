// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Linq.Expressions;
namespace HMI.Runtime.Shared.Common
{
	public static class EventExtensions
	{
		public static void Raise<TEventArgs>(this EventHandler<TEventArgs> @event, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (@event != null)
			{
				@event.Invoke(sender, e);
			}
		}
		public static void Raise(this PropertyChangedEventHandler @event, object sender, PropertyChangedEventArgs e)
		{
			if (@event != null)
			{
				@event.Invoke(sender, e);
			}
		}
		public static void Raise<T>(this PropertyChangedEventHandler @event, object sender, Expression<Func<T>> action)
		{
			string propertyName = EventExtensions.GetPropertyName<T>(action);
			@event.Raise(sender, new PropertyChangedEventArgs(propertyName));
		}
		public static void Raise<TEventArgs>(this EventHandler @event, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (@event != null)
			{
				@event.Invoke(sender, e);
			}
		}
		public static string GetPropertyName<T>(Expression<Func<T>> action)
		{
			MemberExpression memberExpression = (MemberExpression)action.Body;
			return memberExpression.Member.Name;
		}
	}
}
