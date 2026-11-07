// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Presentation.Reactive;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Modules.Design.Resources;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class TemplateSignalBrowserPageViewModel : TemplateBrowserPageViewModel<ISignalParameterDefinition>
	{
		public override string Title
		{
			get
			{
				return BrowserTexts.SignalTemplateParametersBrowserTitle;
			}
		}
		public TemplateSignalBrowserPageViewModel(IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler, ITemplateParametersService templateParametersService) : base(uiScheduler, asyncScheduler, templateParametersService)
		{
		}
		protected override string GetFullCacheKey(string key)
		{
			return "TemplateSignalBrowserPageViewModel." + key;
		}
	}
}
