// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
	public static class BusyOperationEventExtensions
	{
		public static void PublishStartBusyOperation(this IEventAggregator eventAggregator, string busyMessage)
		{
			BusyOperationDetails payload = new BusyOperationDetails(true, busyMessage);
            eventAggregator.Publish<BusyOperationEvent, BusyOperationDetails>(payload);
		}
		public static void PublishEndBusyOperation(this IEventAggregator eventAggregator)
		{
			BusyOperationDetails payload = new BusyOperationDetails(false, null);
            eventAggregator.Publish<BusyOperationEvent, BusyOperationDetails>(payload);
		}
		public static SubscriptionToken SubscribeBusyOperation(this IEventAggregator eventAggregator, Action<BusyOperationDetails> busyActionCallback)
		{
            return eventAggregator.Subscribe<BusyOperationEvent, BusyOperationDetails>(busyActionCallback, ThreadOption.UIThread, false);
		}
	}
}
