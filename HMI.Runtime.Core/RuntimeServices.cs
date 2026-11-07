// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Core;

public static class RuntimeServices
{
    private static IServiceProvider? serviceProvider;

    public static void SetServiceProvider(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        if (Interlocked.CompareExchange(ref serviceProvider, provider, null) is not null)
        {
            throw new InvalidOperationException("The HMI runtime service provider has already been initialized.");
        }
    }

    public static void ClearServiceProvider(IServiceProvider provider)
    {
        Interlocked.CompareExchange(ref serviceProvider, null, provider);
    }

    public static T GetRequiredService<T>() where T : class
    {
        IServiceProvider provider = Volatile.Read(ref serviceProvider)
            ?? throw new InvalidOperationException("The HMI runtime service provider has not been initialized.");

        return provider.GetService(typeof(T)) as T
            ?? throw new InvalidOperationException($"Required runtime service '{typeof(T).FullName}' is not registered.");
    }
}
