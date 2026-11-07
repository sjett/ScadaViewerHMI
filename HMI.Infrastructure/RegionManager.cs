// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace HMI.Infrastructure;

public sealed class RegionManager : IRegionManager
{
    private static RegionManager? current;
    private readonly IServiceProvider services;
    private readonly Dictionary<string, List<Type>> registeredViews = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<WeakReference<ItemsControl>>> regions = new(StringComparer.Ordinal);

    public RegionManager(IServiceProvider services)
    {
        this.services = services;
    }

    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(RegionManager),
            new PropertyMetadata(null, OnRegionNameChanged));

    public static void SetRegionName(DependencyObject element, string? value)
    {
        element.SetValue(RegionNameProperty, value);
    }

    public static string? GetRegionName(DependencyObject element)
    {
        return (string?)element.GetValue(RegionNameProperty);
    }

    public static void SetCurrent(IRegionManager regionManager)
    {
        current = (regionManager as RegionManager) ?? throw new ArgumentNullException(nameof(regionManager));
    }

    public void RegisterViewWithRegion(string regionName, Type viewType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(regionName);
        ArgumentNullException.ThrowIfNull(viewType);

        if (!registeredViews.TryGetValue(regionName, out var views))
        {
            views = new List<Type>();
            registeredViews.Add(regionName, views);
        }

        views.Add(viewType);

        if (regions.TryGetValue(regionName, out var hosts))
        {
            PopulateRegions(regionName, hosts);
        }
    }

    public void UpdateRegions()
    {
    }

    private static void OnRegionNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
    {
        if (element is not ItemsControl itemsControl || args.NewValue is not string regionName)
        {
            return;
        }

        itemsControl.Loaded += (_, _) => current?.AttachRegion(regionName, itemsControl);
        if (itemsControl.IsLoaded)
        {
            current?.AttachRegion(regionName, itemsControl);
        }
    }

    private void AttachRegion(string regionName, ItemsControl itemsControl)
    {
        if (!regions.TryGetValue(regionName, out var hosts))
        {
            hosts = new List<WeakReference<ItemsControl>>();
            regions.Add(regionName, hosts);
        }

        if (hosts.Any(host => host.TryGetTarget(out var existing) && ReferenceEquals(existing, itemsControl)))
        {
            return;
        }

        hosts.RemoveAll(host => !host.TryGetTarget(out _));
        hosts.Add(new WeakReference<ItemsControl>(itemsControl));
        PopulateRegion(regionName, itemsControl);
    }

    private void PopulateRegions(string regionName, List<WeakReference<ItemsControl>> hosts)
    {
        hosts.RemoveAll(host => !host.TryGetTarget(out _));
        foreach (var host in hosts)
        {
            if (host.TryGetTarget(out var itemsControl))
            {
                PopulateRegion(regionName, itemsControl);
            }
        }
    }

    private void PopulateRegion(string regionName, ItemsControl itemsControl)
    {
        if (!registeredViews.TryGetValue(regionName, out var views))
        {
            return;
        }

        foreach (var viewType in views)
        {
            var view = services.GetService(viewType);
            if (view is null)
            {
                if (viewType.IsAbstract || viewType.IsInterface)
                {
                    throw new InvalidOperationException($"The region view '{viewType.FullName}' is not registered.");
                }

                view = ActivatorUtilities.CreateInstance(services, viewType);
            }

            itemsControl.Items.Add(view);
        }
    }
}
