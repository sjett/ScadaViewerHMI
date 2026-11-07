// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Common.Browsers.ViewModels
{
	public class SymbolicTextBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, ISymbolicTextBrowserViewModel
	{
		public SymbolicTextBrowserViewModel(IServerSymbolicTextBrowserViewModel serverSymbolicTextBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverSymbolicTextBrowserViewModel != null, "serverSymbolicTextBrowserViewModel");
            Pages.Add(serverSymbolicTextBrowserViewModel);
		}
	}
}
