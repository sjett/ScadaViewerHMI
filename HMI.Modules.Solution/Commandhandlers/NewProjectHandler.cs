// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using System.Windows.Input;
namespace HMI.Modules.Solution.Commandhandlers
{
	public class NewProjectHandler : WorkflowCommandHandler
	{
		protected override ICommand Command
		{
			get
			{
				return ApplicationCommands.New;
			}
		}
		public NewProjectHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("Close");
			actionNames.Add("ClearProject");
			actionNames.Add("PublishNewProjectEvent");
			actionNames.Add("New");
			actionNames.Add("PrependProjectToMRU");
			actionNames.Add("RefreshMRU");
		}
		protected override bool CanExecute(object state)
		{
			return true;
		}
	}
}
