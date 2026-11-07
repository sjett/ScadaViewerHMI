// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface ITemplateBrowserPageViewModel : ISelectorBrowserPageViewModel<ITemplateParameterViewModel>, IViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel
	{
	}
	public interface ITemplateBrowserPageViewModel<TParameterDefinition> : ISelectorBrowserPageViewModel<ITemplateParameterViewModel>, IViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel, ITemplateBrowserPageViewModel where TParameterDefinition : IParameterDefinition
	{
	}
}
