// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public static class NotificationEventExtensions
	{
		public static void PublishNotification(this IEventAggregator eventAggregator, NotificationSeverity severity, string title, string content, Exception exception)
		{
			NotificationDetails payload = new NotificationDetails(severity, title, content, exception);
            eventAggregator.Publish<NotificationEvent, NotificationDetails>(payload);
		}
		public static SubscriptionToken SubscribeNotification(this IEventAggregator eventAggregator, Action<NotificationDetails> notificationCallback)
		{
            return eventAggregator.Subscribe<NotificationEvent, NotificationDetails>(notificationCallback, ThreadOption.UIThread, false);
		}
	}
}
