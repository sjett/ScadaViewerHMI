// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation;
using HMI.Modules.Application.Actions;
using HMI.Modules.Application.Viewmodels;
namespace HMI.Modules.Application
{
	public class ApplicationModule : PluginModule
	{
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
			base.RegisterTypes(containerRegistry);
            containerRegistry.AddKeyedTransient<IWorkflowAction, PublishApplicationExitEventAction>("PublishApplicationExitEvent");
            containerRegistry.AddKeyedTransient<IWorkflowAction, HideBusyIndicatorAction>("HideBusyIndicator");
            containerRegistry.AddTransient<ISelectLanguageViewModel, SelectLanguageViewModel>();
        }
	}
}
