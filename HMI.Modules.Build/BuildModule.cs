// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.Services.ContentReplacement;
using HMI.Modules.Build.Actions;
using HMI.Modules.Build.CommandHandlers;
using HMI.Modules.Build.Services.Builder;
namespace HMI.Modules.Build
{
	public class BuildModule : PluginModule
	{
		protected override void AddViewsToRegions()
		{
		}
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
            base.RegisterTypes(containerRegistry);

            containerRegistry.AddKeyedTransient<IWorkflowAction, RunProjectAction>("RunProject");
            containerRegistry.AddKeyedTransient<IWorkflowAction, BuildProjectAction>("BuildProject");
            containerRegistry.AddKeyedTransient<IWorkflowAction, ShowBuildingProjectBusyIndicatorAction>("ShowBuildingProjectBusyIndicator");
            containerRegistry.AddKeyedTransient<IWorkflowAction, HideBuildingProjectBusyIndicatorAction>("HideBuildingProjectBusyIndicator");
            containerRegistry.AddKeyedTransient<IWorkflowAction, BuildFailedNotificationAction>("BuildFailedNotification");
            containerRegistry.AddKeyedTransient<ICommandHandler, BuildAndRunProjectHandler>("BuildAndRunProjectHandler");
        }
        protected override void RegisterServices(IServiceCollection containerRegistry)
        {
            containerRegistry.AddTransient<IProjectBuilderService, ProjectBuilderService>();
            containerRegistry.AddTransient<IProjectRunnerService, ProjectRunnerService>();
            containerRegistry.AddTransient<IContentReplacementService, ContentReplacementService>();
        }
	}
}
