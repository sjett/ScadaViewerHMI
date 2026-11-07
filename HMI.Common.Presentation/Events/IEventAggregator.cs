// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events;

public interface IEventAggregator
{
    TEvent GetEvent<TEvent>() where TEvent : EventBase, new();
}

public abstract class EventBase
{
}

public sealed class SubscriptionToken
{
    internal SubscriptionToken(Guid value)
    {
        Value = value;
    }

    internal Guid Value { get; }
}

public enum ThreadOption
{
    PublisherThread,
    UIThread,
    BackgroundThread
}

public sealed class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, EventBase> events = new();

    public TEvent GetEvent<TEvent>() where TEvent : EventBase, new()
    {
        lock (events)
        {
            if (!events.TryGetValue(typeof(TEvent), out var value))
            {
                value = new TEvent();
                events.Add(typeof(TEvent), value);
            }

            return (TEvent)value;
        }
    }
}
