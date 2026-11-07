// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Reflection;

namespace HMI.Common.Presentation.Events;

public class PubSubEvent<TPayload> : EventBase
{
    private readonly object sync = new();
    private readonly Dictionary<Guid, Subscription> subscriptions = new();

    public SubscriptionToken Subscribe(Action<TPayload> action)
    {
        return Subscribe(action, ThreadOption.PublisherThread, false);
    }

    public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberAlive)
    {
        return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberAlive);
    }

    public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
    {
        return Subscribe(action, threadOption, false);
    }

    public SubscriptionToken Subscribe(
        Action<TPayload> action,
        ThreadOption threadOption,
        bool keepSubscriberAlive)
    {
        return Subscribe(action, threadOption, keepSubscriberAlive, _ => true);
    }

    public SubscriptionToken Subscribe(
        Action<TPayload> action,
        ThreadOption threadOption,
        bool keepSubscriberAlive,
        Predicate<TPayload> filter)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(filter);

        var id = Guid.NewGuid();
        lock (sync)
        {
            subscriptions.Add(id, new Subscription(action, threadOption, keepSubscriberAlive, filter));
        }

        return new SubscriptionToken(id);
    }

    public void Unsubscribe(SubscriptionToken? token)
    {
        if (token is null)
        {
            return;
        }

        lock (sync)
        {
            subscriptions.Remove(token.Value);
        }
    }

    public void Unsubscribe(Action<TPayload> action)
    {
        lock (sync)
        {
            foreach (var id in subscriptions
                .Where(entry => entry.Value.Matches(action))
                .Select(entry => entry.Key)
                .ToArray())
            {
                subscriptions.Remove(id);
            }
        }
    }

    public void Publish(TPayload payload)
    {
        KeyValuePair<Guid, Subscription>[] snapshot;
        lock (sync)
        {
            snapshot = subscriptions.ToArray();
        }

        foreach (var (id, subscription) in snapshot)
        {
            if (!subscription.TryGetAction(out var action)
                || !subscription.TryGetFilter(out var filter))
            {
                lock (sync)
                {
                    subscriptions.Remove(id);
                }

                continue;
            }

            if (!filter(payload))
            {
                continue;
            }

            switch (subscription.ThreadOption)
            {
                case ThreadOption.UIThread:
                    var dispatcher = Application.Current?.Dispatcher;
                    if (dispatcher is null || dispatcher.CheckAccess())
                    {
                        action(payload);
                    }
                    else
                    {
                        dispatcher.BeginInvoke(action, payload);
                    }
                    break;
                case ThreadOption.BackgroundThread:
                    _ = Task.Run(() => action(payload));
                    break;
                default:
                    action(payload);
                    break;
            }
        }
    }

    private sealed class Subscription
    {
        private readonly WeakDelegate<Action<TPayload>> action;
        private readonly WeakDelegate<Predicate<TPayload>> filter;

        public Subscription(
            Action<TPayload> action,
            ThreadOption threadOption,
            bool keepSubscriberAlive,
            Predicate<TPayload> filter)
        {
            this.action = new WeakDelegate<Action<TPayload>>(action, keepSubscriberAlive);
            this.filter = new WeakDelegate<Predicate<TPayload>>(filter, keepSubscriberAlive);
            ThreadOption = threadOption;
        }

        public ThreadOption ThreadOption { get; }
        public bool TryGetAction(out Action<TPayload> handler) => action.TryGetDelegate(out handler);
        public bool TryGetFilter(out Predicate<TPayload> predicate) => filter.TryGetDelegate(out predicate);
        public bool Matches(Action<TPayload> handler) => action.Matches(handler);
    }

    private sealed class WeakDelegate<TDelegate> where TDelegate : Delegate
    {
        private readonly TDelegate? strongDelegate;
        private readonly WeakReference<object>? target;
        private readonly MethodInfo? method;

        public WeakDelegate(TDelegate handler, bool keepAlive)
        {
            if (keepAlive || handler.Target is null)
            {
                strongDelegate = handler;
                return;
            }

            target = new WeakReference<object>(handler.Target);
            method = handler.Method;
        }

        public bool TryGetDelegate(out TDelegate handler)
        {
            if (strongDelegate is not null)
            {
                handler = strongDelegate;
                return true;
            }

            if (target is not null && target.TryGetTarget(out var instance))
            {
                handler = (TDelegate)method!.CreateDelegate(typeof(TDelegate), instance);
                return true;
            }

            handler = null!;
            return false;
        }

        public bool Matches(TDelegate handler)
        {
            if (strongDelegate is not null)
            {
                return strongDelegate == handler;
            }

            return method == handler.Method
                && target is not null
                && target.TryGetTarget(out var instance)
                && ReferenceEquals(instance, handler.Target);
        }
    }
}
