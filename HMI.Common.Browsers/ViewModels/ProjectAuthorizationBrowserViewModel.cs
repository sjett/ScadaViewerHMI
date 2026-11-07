// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Common.Browsers.ViewModels
{
	public class ProjectAuthorizationBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, IProjectAuthorizationBrowserViewModel
	{
		public ProjectAuthorizationBrowserViewModel(IServerProjectAuthorizationBrowserViewModel serverProjectAuthorizationBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverProjectAuthorizationBrowserViewModel != null, "serverProjectAuthorizationBrowserViewModel");
            Pages.Add(serverProjectAuthorizationBrowserViewModel);
		}
	}
}
