// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using HMI.Common.Instrumentation.Metadata;

namespace HMI.Common.Instrumentation.Events
{
    public static class EventExtensions
    {
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> @event, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (@event != null)
            {
                @event(sender, e);
            }
        }
        public static void Raise(this PropertyChangedEventHandler @event, object sender, PropertyChangedEventArgs e)
        {
            if (@event != null)
            {
                @event(sender, e);
            }
        }
        public static void Raise<T>(this PropertyChangedEventHandler @event, object sender, Expression<Func<T>> action)
        {
            if (@event == null)
            {
                return;
            }
            string propertyName = Reflection.GetPropertyName<T>(action);
            @event.Raise(sender, new PropertyChangedEventArgs(propertyName));
        }
        public static void Raise(this PropertyChangingEventHandler @event, object sender, PropertyChangingEventArgs e)
        {
            if (@event != null)
            {
                @event(sender, e);
            }
        }
        public static void Raise<T>(this PropertyChangingEventHandler @event, object sender, Expression<Func<T>> action)
        {
            if (@event == null)
            {
                return;
            }
            string propertyName = Reflection.GetPropertyName<T>(action);
            @event.Raise(sender, new PropertyChangingEventArgs(propertyName));
        }
        public static void Raise<TEventArgs>(this EventHandler @event, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (@event != null)
            {
                @event(sender, e);
            }
        }
        public static void Raise(this CancelEventHandler @event, object sender, CancelEventArgs e)
        {
            if (@event != null)
            {
                @event(sender, e);
            }
        }
    }
}
