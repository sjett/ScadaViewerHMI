// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Services;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Browsers.Views;
using HMI.Common.Browsers.Views.Serialization;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence.Services;

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

namespace HMI.Common.Browsers;

public sealed class BrowsersModule : IHmiModule
{
    public void RegisterTypes(IServiceCollection services)
    {
        services.AddSingleton<IUiScheduler, UiScheduler>();
        services.AddSingleton<IAsyncScheduler, AsyncScheduler>();
        services.AddTransient<SignalDefinitionBrowser>();
        services.AddTransient<IServerSymbolicTextBrowserViewModel, ServerSymbolicTextBrowserViewModel>();
        services.AddTransient<IServerAlarmGroupBrowserViewModel, ServerAlarmGroupBrowserViewModel>();
        services.AddTransient<IServerAlarmGroupTreeBrowserViewModel, ServerAlarmGroupTreeBrowserViewModel>();
        services.AddTransient<IServerAlarmTypeBrowserViewModel, ServerAlarmTypeBrowserViewModel>();
        services.AddTransient<IServerAlarmTypeTreeBrowserViewModel, ServerAlarmTypeTreeBrowserViewModel>();
        services.AddTransient<IServerProjectAuthorizationBrowserViewModel, ServerProjectAuthorizationBrowserViewModel>();
        services.AddTransient<IServerSignalDefinitionBrowserViewModel, ServerSignalDefinitionBrowserViewModel>();
        services.AddTransient<ISignalDefinitionBrowserViewModel, SignalDefinitionBrowserViewModel>();
        services.AddTransient<ISymbolicTextBrowserViewModel, SymbolicTextBrowserViewModel>();
        services.AddTransient<IAlarmGroupBrowserViewModel, AlarmGroupBrowserViewModel>();
        services.AddTransient<IAlarmGroupTreeBrowserViewModel, AlarmGroupTreeBrowserViewModel>();
        services.AddTransient<IAlarmTypeBrowserViewModel, AlarmTypeBrowserViewModel>();
        services.AddTransient<IAlarmTypeTreeBrowserViewModel, AlarmTypeTreeBrowserViewModel>();
        services.AddTransient<IProjectAuthorizationBrowserViewModel, ProjectAuthorizationBrowserViewModel>();
        services.AddSingleton<IClientAccessor, ClientAccessor>();
        services.AddTransient<ISignalsService, CachedSignalsService>();
        services.AddTransient<ISymbolicTextService, CachedSymbolicTextService>();
        services.AddTransient<IAlarmService, AlarmService>();
        services.AddTransient<IProjectAuthorizationService, ProjectAuthorizationService>();
    }

    public void OnInitialized(System.IServiceProvider services)
    {
        var regionManager = services.GetRequiredService<IRegionManager>();
        regionManager.RegisterViewWithRegion("OKCancelRefreshButtonPanel", typeof(OKCancelRefreshPanelView));
        regionManager.RegisterViewWithRegion("OKCancelButtonPanel", typeof(OKCancelPanelView));
        regionManager.RegisterViewWithRegion("GenericBrowserStatusBar", typeof(GenericBrowserStatusBarView));
        regionManager.RegisterViewWithRegion("AlarmFilterTreeBrowserToolbar", typeof(AlarmFilterTreeBrowserToolbarView));
        Telerik.Windows.Persistence.Services.ServiceProvider.RegisterPersistenceProvider<ICustomPropertyProvider>(
            typeof(RadGridView),
            new GridViewCustomPropertyProvider());
    }
}
