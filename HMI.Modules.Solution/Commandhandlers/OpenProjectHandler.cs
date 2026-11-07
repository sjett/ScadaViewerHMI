// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using System.Windows.Input;
namespace HMI.Modules.Solution.Commandhandlers
{
	public class OpenProjectHandler : WorkflowCommandHandler
	{
		protected override ICommand Command
		{
			get
			{
				return ApplicationCommands.Open;
			}
		}
		public OpenProjectHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override bool CanExecute(object state)
		{
			return true;
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("Open");
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
	}
}
