// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure;
using HMI.Infrastructure.Commanding;
using System.Windows.Input;
namespace HMI.Modules.Solution.Commandhandlers
{
	public class SelectLanguageHandler : WorkflowCommandHandler
	{
		protected override ICommand Command
		{
			get
			{
				return EditorCommands.SelectLanguage;
			}
		}
		public SelectLanguageHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		protected override bool CanExecute(object state)
		{
			return true;
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("SelectLanguage");
		}
	}
}
