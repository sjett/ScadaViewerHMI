// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Solution.Resources;
using HMI.Infrastructure.Commanding;
using System.Windows;

namespace HMI.Modules.Solution.Actions
{
    public class CloseProjectAction : SolutionAction, IWorkflowAction
	{
		public CloseProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			if (Project == null)
			{
				return true;
			}
			MessageBoxResult savePromptResult = PromptToSaveProject();
			if (savePromptResult != MessageBoxResult.Cancel)
			{
				if (savePromptResult == MessageBoxResult.Yes)
				{
					AddSaveAction(workflow);
				}
				return true;
			}
			return false;
		}
        private MessageBoxResult PromptToSaveProject()
        {
            return this.NotificationService.Value.ShowYesNoCancelMessageBox(ProjectActionsTexts.SaveProjectBeforeClosing_Title, ProjectActionsTexts.SaveProjectBeforeClosing_Caption);
        }
        private void AddSaveAction(IWorkflow workflow)
        {
            this.InsertWorkflowAction("Save", workflow);
        }
    }
}
