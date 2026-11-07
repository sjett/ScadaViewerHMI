// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Services;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Presentation
{
    public abstract class PluginModule : ModuleBase
    {
        protected IMergeDictionaryRegistry MergeDictionaryRegistry { get; private set; } = null!;
        protected IRegionManager RegionManager { get; private set; } = null!;
        protected ISerializerFactory SerializerFactory { get; private set; } = null!;

        public override void OnInitialized(IServiceProvider services)
        {
            base.OnInitialized(services);

            RegionManager = services.GetRequiredService<IRegionManager>();
            SerializerFactory = services.GetRequiredService<ISerializerFactory>();
            MergeDictionaryRegistry = services.GetRequiredService<IMergeDictionaryRegistry>();

            this.RegisterMergeDictionaries();
            this.RegisterSerializers(this.SerializerFactory);
            this.AddViewsToRegions();
        }
        public override void RegisterTypes(IServiceCollection containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            this.RegisterViewModels(containerRegistry);
            this.RegisterViews(containerRegistry);
        }
        protected virtual void RegisterViews(IServiceCollection containerRegistry)
        {
        }
        protected virtual void RegisterViewModels(IServiceCollection containerRegistry)
        {
        }

        protected virtual void RegisterSerializers(ISerializerFactory serializerFactory)
        {
        }
        protected virtual void RegisterMergeDictionaries()
        {
        }
        protected virtual void AddViewsToRegions()
        {
        }
        protected void RegisterMergeDictionary(string assemblyName, string assemblyRelativeResourceDictionaryPath)
        {
            MergeDictionaryRegistry.AddDictionaryResource(new Uri($"pack://application:,,,/{assemblyName};component{assemblyRelativeResourceDictionaryPath}"));
        }
    }
}
