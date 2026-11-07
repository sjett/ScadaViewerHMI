// SPDX-License-Identifier: GPL-3.0-only
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using HMI.Common.Browsers.Services;
using HMI.Common.Browsers.ViewModels;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure;
using HMI.Modules.Design.ViewModels.Browsers;
using HMI.Modules.Design.Views.Browsers;

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

namespace HMI.Modules.Design;

[ExcludeFromCodeCoverage]
public sealed class DesignModule : ModuleBase
{
    public override void RegisterTypes(IServiceCollection services)
    {
        services.AddTransient<IServiceConfiguration, ServiceConfiguration>();
        services.AddTransient<IMediaBrowserPageViewModel<IImageSourceViewModel>, ImageBrowserPageViewModel>();
        services.AddTransient<IMediaBrowserPageViewModel<IAudioSourceViewModel>, AudioBrowserPageViewModel>();
        services.AddTransient<IMediaBrowserPageViewModel<IXamlSourceViewModel>, XamlBrowserPageViewModel>();
        services.AddTransient<ImageBrowser>();
        services.AddTransient<AudioBrowser>();
        services.AddTransient<XamlBrowser>();
        services.AddTransient<IHorizontalCollectionBrowserViewModel, HorizontalCollectionBrowserViewModel>();
        services.AddTransient<IVerticalCollectionBrowserViewModel, VerticalCollectionBrowserViewModel>();
        services.AddTransient(typeof(ISelectorBrowserViewModel<>), typeof(SelectorBrowserViewModel<>));
        services.AddTransient<ISignalDefinitionBrowserViewModel, TemplatableSignalDefinitionBrowserViewModel>();
        services.AddTransient<ISymbolicTextBrowserViewModel, TemplatableSymbolicTextBrowserViewModel>();
        services.AddTransient<IAlarmGroupBrowserViewModel, TemplatableAlarmGroupBrowserViewModel>();
        services.AddTransient<IAlarmTypeBrowserViewModel, TemplatableAlarmTypeBrowserViewModel>();
        services.AddTransient<IAlarmGroupTreeBrowserViewModel, TemplatableAlarmGroupTreeBrowserViewModel>();
        services.AddTransient<IAlarmTypeTreeBrowserViewModel, TemplatableAlarmTypeTreeBrowserViewModel>();
        services.AddTransient<ITemplateBrowserPageViewModel<ISignalParameterDefinition>, TemplateSignalBrowserPageViewModel>();
        services.AddTransient<ITemplateBrowserPageViewModel<ISymbolicTextParameterDefinition>, TemplateSymbolicTextBrowserPageViewModel>();
        services.AddTransient<ITemplateBrowserPageViewModel<IAlarmGroupParameterDefinition>, TemplateAlarmGroupBrowserPageViewModel>();
        services.AddTransient<ITemplateBrowserPageViewModel<IAlarmTypeParameterDefinition>, TemplateAlarmTypeBrowserPageViewModel>();
        services.AddTransient<ITemplateParameterViewModel, TemplateParameterDefinitionViewModel>();
    }

    public override void OnInitialized(IServiceProvider services)
    {
        base.OnInitialized(services);

        var regionManager = services.GetRequiredService<IRegionManager>();
        regionManager.RegisterViewWithRegion("VerticalCollectionBrowserView", typeof(VerticalCollectionBrowserView));
        regionManager.RegisterViewWithRegion("HorizontalCollectionBrowserView", typeof(HorizontalCollectionBrowserView));
        regionManager.RegisterViewWithRegion("CollectionBrowserButtonsRegion", typeof(CollectionBrowserButtonsView));
    }
}
