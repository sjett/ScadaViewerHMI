// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation;

namespace HMI.Infrastructure
{
    public abstract class ModuleBase : IHmiModule
    {
        protected IServiceProvider Container { get; private set; } = null!;

        public virtual void RegisterTypes(IServiceCollection services)
        {
            RegisterServices(services);
        }

        public virtual void OnInitialized(IServiceProvider services)
        {
            Container = services;
        }

        protected virtual void RegisterServices(IServiceCollection services)
        {
        }
    }
}
