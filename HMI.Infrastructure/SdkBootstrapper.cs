// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;

namespace HMI.Infrastructure
{
    public static class SdkBootstrapper
	{
        public static void ConfigureContainer(IServiceCollection containerRegistry)
        {
            containerRegistry.AddTransient<IWorkflow, Workflow>();
            containerRegistry.AddSingleton<ITaskRunner, TaskRunner>();
        }
	}
}
