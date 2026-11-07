// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Modules.Solution.Actions
{
    public class NewProjectAction : SolutionAction, IWorkflowAction
	{
		public NewProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			string projectFolder;
			string projectName;
			if (!TrySelectNewProjectDetails(out projectFolder, out projectName))
			{
				return false;
			}
            Project = CreateProject(projectFolder, projectName);
			return true;
		}
        private bool TrySelectNewProjectDetails(out string projectFolder, out string projectName)
        {
            return this.NotificationService.Value.ShowNewProjectDialog(out projectFolder, out projectName);
        }
        private IProjectViewModel CreateProject(string projectFolder, string projectName)
        {
            return this.ProjectManagerService.Value.NewProject(projectFolder, projectName);
        }
    }
}
