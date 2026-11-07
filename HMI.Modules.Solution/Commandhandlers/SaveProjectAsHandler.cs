// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Windows.Input;
namespace HMI.Modules.Solution.Commandhandlers
{
	public class SaveProjectAsHandler : WorkflowCommandHandler
	{
		
		protected Lazy<IProjectManagerViewModel> Solution
		{
			get;
			private set;
		}
		private IProjectViewModel Project
		{
			get
			{
				return Solution.Value.Project;
			}
		}
		protected override ICommand Command
		{
			get
			{
				return ApplicationCommands.SaveAs;
			}
		}
		public SaveProjectAsHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            Solution = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
		}
		protected override bool CanExecute(object state)
		{
			return Project != null;
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("SaveAs");
			actionNames.Add("PrependProjectToMRU");
			actionNames.Add("RefreshMRU");
		}
	}
}
