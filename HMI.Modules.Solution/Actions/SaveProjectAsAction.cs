// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;

namespace HMI.Modules.Solution.Actions
{
    public class SaveProjectAsAction : SolutionAction
	{
		public SaveProjectAsAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			string projectFolder;
			string projectName;
			if (!NotificationService.Value.ShowSaveProjectAsDialog(out projectFolder, out projectName))
			{
				return false;
			}
            ProjectManagerService.Value.SaveAs(Project, projectFolder, projectName);
			return true;
		}
	}
}
