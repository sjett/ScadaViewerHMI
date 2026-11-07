// SPDX-License-Identifier: GPL-3.0-only
using Microsoft.Extensions.DependencyInjection;

namespace HMI.Common.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static T Resolve<T>(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var type = typeof(T);
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Func<>))
        {
            var serviceType = type.GetGenericArguments()[0];
            return (T)typeof(ServiceProviderExtensions)
                .GetMethod(nameof(CreateFactory), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(serviceType)
                .Invoke(null, [services])!;
        }

        return services.GetRequiredService<T>();
    }

    public static T Resolve<T>(this IServiceProvider services, string serviceKey)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceKey);
        return services.GetRequiredKeyedService<T>(serviceKey);
    }

    public static IEnumerable<T> ResolveAll<T>(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return services.GetServices<T>().Concat(services.GetKeyedServices<T>(KeyedService.AnyKey));
    }

    private static Func<TService> CreateFactory<TService>(IServiceProvider services)
    {
        return () => services.GetRequiredService<TService>();
    }
}
