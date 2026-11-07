// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.Services.UndoRedo;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Infrastructure.Services;
namespace HMI.Infrastructure.Presentation
{
    public class PresentationModule : PluginModule
    {
        public override void RegisterTypes(IServiceCollection containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            containerRegistry.AddSingleton<IParameterReferenceMonitorFactory, ParameterReferenceMonitorFactory>();
        }
        protected override void RegisterViewModels(IServiceCollection containerRegistry)
        {
            base.RegisterViewModels(containerRegistry);

            containerRegistry.AddTransient<ITemplatePageViewModel, TemplatePageViewModel>();
            containerRegistry.AddTransient<IProjectPageViewModel, ProjectPageViewModel>();
            containerRegistry.AddSingleton<IGuideSettingsViewModel, GuideSettingsViewModel>();
            containerRegistry.AddTransient<IProjectViewModel, ProjectViewModel>();
            containerRegistry.AddTransient<ISelectionControlViewModel, SelectionControlViewModel>();
        }
        protected override void RegisterServices(IServiceCollection containerRegistry)
        {
            base.RegisterServices(containerRegistry);

            containerRegistry.AddSingleton<INavigationTargetService, NavigationTargetService>();
            containerRegistry.AddSingleton<IActionMonitor, ActionMonitor>();
            containerRegistry.AddTransient(typeof(ICollectionMonitor<>), typeof(CollectionMonitor<>));
            containerRegistry.AddTransient<IPropertyMonitor, PropertyMonitor>();
            containerRegistry.AddTransient<IPageChildrenService, PageChildrenService>();
            containerRegistry.AddTransient<IPageSelectionService, PageSelectionService>();
            containerRegistry.AddTransient<IPageUndoRedoService, PageUndoRedoService>();
            containerRegistry.AddSingleton<IProjectNameService, ProjectNameService>();
            containerRegistry.AddSingleton(typeof(ICache<,>), typeof(CacheService<,>));
            containerRegistry.AddTransient<IPageBuilderService, PageBuilderService>();
            containerRegistry.AddTransient<ISignalResolverService, SignalResolverService>();
            containerRegistry.AddTransient<ITemplateParametersService, TemplateParametersService>();
            containerRegistry.AddTransient<IZOrderService, ZOrderService>();
        }
        protected override void RegisterSerializers(ISerializerFactory serializerFactory)
        {
            base.RegisterSerializers(serializerFactory);
            serializerFactory.RegisterSerializer<IProjectViewModel, ProjectSerializer>();
            serializerFactory.RegisterSerializer<ICommonControlViewModel, CommonControlSerializer>();
            serializerFactory.RegisterSerializer<GroupControl, ControlGroupContainerSerializer>();
            serializerFactory.RegisterSerializer<ViewModels.ParameterValue, ParameterValueSerializer>();
            serializerFactory.RegisterSerializer<ITemplatePageViewModel, TemplatePageSerializer>();
            serializerFactory.RegisterSerializer<IProjectPageViewModel, PageSerializer>();

        }
        protected override void RegisterMergeDictionaries()
        {
            base.RegisterMergeDictionaries();

            base.RegisterMergeDictionary(base.GetType().Assembly.FullName, "/Views/SelectionControl.xaml");
            base.RegisterMergeDictionary(base.GetType().Assembly.FullName, "/Views/GroupControl.xaml");
        }
    }

    
}
