// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Metadata;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace HMI.Common.Presentation.Reactive
{
	public static class NotifyPropertyChangeReactiveExtensions
	{
		public static IObservable<TValue> FromPropertyValues<TSource, TValue>(this TSource source, Expression<Func<TSource, TValue>> propertyExpression) where TSource : INotifyPropertyChanged
		{
			var pname = Reflection.GetPropertyName(propertyExpression); 
			var compiled = propertyExpression.Compile();

			return source.FromPropertyChangedEvents()
				.Where(t => t.EventArgs.PropertyName == pname)
				.Select(t => compiled(source))
                .StartWith(
                [
                    compiled(source)
				]);
		}
		public static IObservable<EventPattern<PropertyChangedEventArgs>> FromPropertyChanged<TSource, TValue>(this TSource source, Expression<Func<TSource, TValue>> propertyExpression) where TSource : INotifyPropertyChanged
		{
			var pname = Reflection.GetPropertyName(propertyExpression);
			return source.FromPropertyChangedEvents()
				.Where(t=> t.EventArgs.PropertyName == pname);
		}
		public static IObservable<EventPattern<PropertyChangingEventArgs>> FromPropertyChanging<TSource, TValue>(this TSource source, Expression<Func<TSource, TValue>> propertyExpression) where TSource : INotifyPropertyChanging
		{
			var pname = Reflection.GetPropertyName(propertyExpression);

			return source.FromPropertyChangingEvents()
				.Where(t=> t.EventArgs.PropertyName == pname);
		}
		public static IObservable<EventPattern<PropertyChangedEventArgs>> FromPropertyChangedEvents(this INotifyPropertyChanged source)
		{
			return Observable
				.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				addHandler => source.PropertyChanged += addHandler, 
				removeHandler => source.PropertyChanged -= removeHandler);
		}
		public static IObservable<EventPattern<PropertyChangingEventArgs>> FromPropertyChangingEvents(this INotifyPropertyChanging source)
		{
			return Observable
				.FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(
				addHandler => source.PropertyChanging += addHandler, 
				removeHandler => source.PropertyChanging -= removeHandler);
		}
	}
}
