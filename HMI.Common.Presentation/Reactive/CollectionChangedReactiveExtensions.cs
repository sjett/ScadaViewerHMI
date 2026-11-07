// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
namespace HMI.Common.Presentation.Reactive
{
	public static class CollectionChangedReactiveExtensions
	{
		public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> FromCollectionChangedEventPattern(this INotifyCollectionChanged collection)
		{
			return Observable
					.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
						addHandler => collection.CollectionChanged += addHandler,
						removeHandler => collection.CollectionChanged -= removeHandler);
		}
	}
}
