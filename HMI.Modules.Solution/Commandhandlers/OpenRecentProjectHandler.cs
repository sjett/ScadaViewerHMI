// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows.Input;
namespace HMI.Modules.Solution.Commandhandlers
{
	public class OpenRecentProjectHandler : WorkflowCommandHandler
	{
		protected Lazy<IProjectManagerViewModel> Solution
		{
			get;
			private set;
		}
		protected override ICommand Command
		{
			get
			{
				return EditorCommands.OpenMruItem;
			}
		}
		public OpenRecentProjectHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            Solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			return Solution.Value.MruItems.Any<MruEntry>();
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("Close");
			actionNames.Add("ClearProject");
			actionNames.Add("ShowLoadingProjectBusyIndicator");
			actionNames.Add("PublishNewProjectEvent");
			actionNames.Add("Load");
			actionNames.Add("ActivateProject");
			actionNames.Add("PrependProjectToMRU");
			actionNames.Add("RefreshMRU");
			actionNames.Add("HideBusyIndicator");
		}
		protected override void ConfigureWorkflow(IWorkflow workflow, object state)
		{
			base.ConfigureWorkflow(workflow, null);
			MruEntry mruEntry = (MruEntry)state;
			workflow.Data["projectFileName"] = mruEntry.Path;
		}
	}
}
