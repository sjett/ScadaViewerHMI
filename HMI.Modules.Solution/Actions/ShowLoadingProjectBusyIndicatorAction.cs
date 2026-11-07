// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Application.Actions;
using HMI.Modules.Solution.Resources;
namespace HMI.Modules.Solution.Actions
{
	public class ShowLoadingProjectBusyIndicatorAction : ShowBusyIndicatorAction
	{
		protected override string Message
		{
			get
			{
				return ProjectActionsTexts.LoadingProjectMessage;
			}
		}
		public ShowLoadingProjectBusyIndicatorAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
	}
}
