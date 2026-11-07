// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class SignalDefinitionBrowserViewModel : SelectorBrowserViewModel, INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel, ISignalDefinitionBrowserViewModel
	{
		public SignalDefinitionBrowserViewModel(IServerSignalDefinitionBrowserViewModel serverSignalDefinitionBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(serverSignalDefinitionBrowserViewModel != null, "serverSignalDefinitionBrowserViewModel");
            Pages.Add(serverSignalDefinitionBrowserViewModel);
		}
	}
}
