// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation;
using HMI.Modules.Solution.Actions;
using HMI.Modules.Solution.Commandhandlers;
namespace HMI.Modules.Solution
{
	public class SolutionModule : PluginModule
	{
		public override void RegisterTypes(IServiceCollection containerRegistry)
		{
			base.RegisterTypes(containerRegistry);

            containerRegistry.AddKeyedTransient<IWorkflowAction, CloseProjectAction>("Close");
            containerRegistry.AddKeyedTransient<IWorkflowAction, ClearProjectAction>("ClearProject");
            containerRegistry.AddKeyedTransient<IWorkflowAction, LoadProjectAction>("Load");
            containerRegistry.AddKeyedTransient<IWorkflowAction, ActivateProjectAction>("ActivateProject");
            containerRegistry.AddKeyedTransient<IWorkflowAction, NewProjectAction>("New");
            containerRegistry.AddKeyedTransient<IWorkflowAction, OpenProjectAction>("Open");
            containerRegistry.AddKeyedTransient<IWorkflowAction, PrependCurrentProjectToMRUAction>("PrependProjectToMRU");
            containerRegistry.AddKeyedTransient<IWorkflowAction, RefreshMRUAction>("RefreshMRU");
            containerRegistry.AddKeyedTransient<IWorkflowAction, SaveProjectAction>("Save");
            containerRegistry.AddKeyedTransient<IWorkflowAction, SaveProjectAsAction>("SaveAs");
            containerRegistry.AddKeyedTransient<IWorkflowAction, PublishNewProjectEventAction>("PublishNewProjectEvent");
            containerRegistry.AddKeyedTransient<IWorkflowAction, ShowLoadingProjectBusyIndicatorAction>("ShowLoadingProjectBusyIndicator");
            containerRegistry.AddKeyedTransient<IWorkflowAction, BeginUndoGroupAction>("BeginUndoGroup");
            containerRegistry.AddKeyedTransient<IWorkflowAction, EndUndoGroupAction>("EndUndoGroup");
            containerRegistry.AddKeyedTransient<IWorkflowAction, SelectLanguageAction>("SelectLanguage");
            
            containerRegistry.AddKeyedTransient<ICommandHandler, NewProjectHandler>("NewProjectCommandHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, OpenProjectHandler>("OpenProjectCommandHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, SaveProjectHandler>("SaveProjectCommandHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, SaveProjectAsHandler>("SaveProjectAsCommandHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, OpenRecentProjectHandler>("OpenRecentProjectCommandHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, ExitApplicationHandler>("ExitApplicationHandler");
            containerRegistry.AddKeyedTransient<ICommandHandler, SelectLanguageHandler>("SelectLanguageHandler");
        }
	}
}
