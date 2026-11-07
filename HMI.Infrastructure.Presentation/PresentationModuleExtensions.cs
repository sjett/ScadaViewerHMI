// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.Services.UndoRedo;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HMI.Infrastructure.Presentation
{
    public static class PresentationModuleExtensions
    {
        public static IServiceCollection AddPresentationModule(this IServiceCollection services)
        {
            //Register Types
            services.AddSingleton<IParameterReferenceMonitorFactory, ParameterReferenceMonitorFactory>();
            //Register Services
            services.AddSingleton<INavigationTargetService, NavigationTargetService>();
            services.AddSingleton<IActionMonitor, ActionMonitor>();
            services.AddSingleton(typeof(ICollectionMonitor<>), typeof(CollectionMonitor<>));
            services.AddSingleton<IPropertyMonitor, PropertyMonitor>();
            services.AddSingleton<IPageChildrenService, PageChildrenService>();
            services.AddSingleton<IPageSelectionService, PageSelectionService>();
            services.AddSingleton<IPageUndoRedoService, PageUndoRedoService>();
            services.AddSingleton<IProjectNameService, ProjectNameService>();
            services.AddSingleton(typeof(ICache<,>), typeof(CacheService<,>));
            services.AddSingleton<IPageBuilderService, PageBuilderService>();
            services.AddSingleton<ISignalResolverService, SignalResolverService>();
            services.AddSingleton<ITemplateParametersService, TemplateParametersService>();
            services.AddSingleton<IZOrderService, ZOrderService>();
            //Register ViewModels
            services.AddSingleton<ITemplatePageViewModel, TemplatePageViewModel>();
            services.AddSingleton<IProjectPageViewModel, ProjectPageViewModel>();
            services.AddSingleton<IGuideSettingsViewModel, GuideSettingsViewModel>();
            services.AddSingleton<IProjectViewModel, ProjectViewModel>();
            services.AddSingleton<ISelectionControlViewModel, SelectionControlViewModel>();

            //Register Serializers
            services.RegisterSerializer<IProjectViewModel, ProjectSerializer>();
            services.RegisterSerializer<ICommonControlViewModel, CommonControlSerializer>();
            services.RegisterSerializer<GroupControl, ControlGroupContainerSerializer>();
            services.RegisterSerializer<ParameterValue, ParameterValueSerializer>();
            services.RegisterSerializer<ITemplatePageViewModel, TemplatePageSerializer>();
            services.RegisterSerializer<IProjectPageViewModel, PageSerializer>();

            return services;
        }

        public static IServiceCollection RegisterSerializer<TFor, TSerializer>(this IServiceCollection services)
            //where TFor : class 
            where TSerializer : class, ISerializer 
        {
            services.AddKeyedSingleton<ISerializer, TSerializer>(typeof(TFor).FullName);

            return services;
        }
        public static IServiceCollection RegisterSerializer<TFor>(this IServiceCollection services, ISerializer instance)
        {
            services.AddKeyedSingleton(typeof(TFor).FullName, instance);

            return services;
        }

        public static IMergeDictionaryRegistry MergePresentationModuleDictionaries(this IMergeDictionaryRegistry mergeDictionaryRegistry)
        {
            var assembly = Assembly.GetExecutingAssembly().FullName;
            mergeDictionaryRegistry.AddDictionaryResource(new Uri(string.Format("pack://application:,,,/{0};component{1}",
                assembly, "/Views/SelectionControl.xaml")));
            mergeDictionaryRegistry.AddDictionaryResource(new Uri(string.Format("pack://application:,,,/{0};component{1}",
                assembly, "/Views/GroupControl.xaml")));

            return mergeDictionaryRegistry;
        }

    }
}
