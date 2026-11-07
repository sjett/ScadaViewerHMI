// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Solution.Resources;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;
namespace HMI.Modules.Solution.Actions
{
	public class SelectLanguageAction : SolutionAction
	{
		private readonly ILanguageService _languageService;
		public SelectLanguageAction(IServiceProvider serviceLocator, ILanguageService languageService) : base(serviceLocator)
		{
            _languageService = languageService;
		}
		public override bool Execute(IWorkflow workflow)
		{
			string selectedLanguage = GetSelectedLanguage();
			if (HasLanguageChanged(selectedLanguage))
			{
                _languageService.ChangeLanguage(selectedLanguage);
                NotificationService.Value.ShowOKMessageBox(ProjectActionsTexts.SelectLanguageTitle, ProjectActionsTexts.SelectLanguageText);
			}
			return true;
		}
        private string GetSelectedLanguage()
        {
            string existingLanguage = _languageService.GetLanguage();
            return this.NotificationService.Value.GetNewLanguage(ProjectActionsTexts.SelectLanguageTitle, existingLanguage);
        }
        private bool HasLanguageChanged(string selectedLanguage)
        {
            return selectedLanguage != null && selectedLanguage != _languageService.GetLanguage();
        }
    }
}
