// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events
{
	public static class EventAggregatorExtensions
	{
		public static SubscriptionToken Subscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				return @event.Subscribe(action);
			}
			return null;
		}
		public static SubscriptionToken Subscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action, ThreadOption threadOption) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				return @event.Subscribe(action, threadOption);
			}
			return null;
		}
		public static SubscriptionToken Subscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action, bool keepSubscriberAlive) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				return @event.Subscribe(action, keepSubscriberAlive);
			}
			return null;
		}
		public static SubscriptionToken Subscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberAlive) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				return @event.Subscribe(action, threadOption, keepSubscriberAlive);
			}
			return null;
		}
		public static SubscriptionToken Subscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberAlive, Predicate<TPayload> filter) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				return @event.Subscribe(action, threadOption, keepSubscriberAlive, filter);
			}
			return null;
		}
		public static void Unsubscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, SubscriptionToken token) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				@event.Unsubscribe(token);
			}
		}
		public static void Unsubscribe<TEvent, TPayload>(this IEventAggregator eventAggregator, Action<TPayload> action) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				@event.Unsubscribe(action);
			}
		}
		public static void Publish<TEvent, TPayload>(this IEventAggregator eventAggregator, TPayload payload) where TEvent : PubSubEvent<TPayload>, new()
		{
			TEvent @event = eventAggregator.GetEvent<TEvent>();
			if (@event != null)
			{
				@event.Publish(payload);
			}
		}
	}
}
