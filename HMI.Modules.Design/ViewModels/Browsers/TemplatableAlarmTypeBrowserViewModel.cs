// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class TemplatableAlarmTypeBrowserViewModel : AlarmTypeBrowserViewModel, IBindingContextAware
	{
		private IBindingSource _bindingContext;
		public IBindingSource Context
		{
			get
			{
				return this._bindingContext;
			}
			set
			{
				this._bindingContext = value;
				foreach (ISelectorBrowserPageViewModel current in base.Pages)
				{
					IBindingContextAware bindingContextAware = current as IBindingContextAware;
					if (bindingContextAware != null)
					{
						bindingContextAware.Context = value;
					}
				}
				base.RaisePropertyChanged<bool>(() => this.AreMultiplePagesVisible);
				base.RaisePropertyChanged<ISelectorBrowserPageViewModel>(() => this.ActivePage);
			}
		}
		public TemplatableAlarmTypeBrowserViewModel(IServerAlarmTypeBrowserViewModel serverAlarmTypeBrowserViewModel, ITemplateBrowserPageViewModel<IAlarmTypeParameterDefinition> templateBrowserPageViewModel) : base(serverAlarmTypeBrowserViewModel)
		{
			Contract.Requires<ArgumentNullException>(templateBrowserPageViewModel != null, "templateBrowserPageViewModel");
			base.Pages.Insert(0, templateBrowserPageViewModel);
		}
	}
}
