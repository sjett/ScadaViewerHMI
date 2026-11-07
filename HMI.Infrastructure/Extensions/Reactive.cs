// SPDX-License-Identifier: GPL-3.0-only
using System.Reactive.Linq;

namespace HMI.Infrastructure.Extensions
{
    public static class Reactive
    {
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> stream, TimeSpan delay)
        {
            IObservable<T> inactivitySignal = stream.Throttle(delay);
            return stream
                .Window(() => inactivitySignal)
                .SelectMany(BufferWindow);
        }
        
        private static IObservable<IList<T>> BufferWindow<T>(IObservable<T> window)
        {
            return window.ToList<T>();
        }
    }
}
