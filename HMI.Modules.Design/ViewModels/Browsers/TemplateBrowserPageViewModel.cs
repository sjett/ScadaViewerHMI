// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public abstract class TemplateBrowserPageViewModel<TParameterDefinition> : SelectorBrowserPageViewModel<TParameterDefinition, ITemplateParameterViewModel>, IBindingContextAware, ISelectorBrowserPageViewModel<ITemplateParameterViewModel>, ITemplateBrowserPageViewModel<TParameterDefinition>, IViewModel, INotifyPropertyChanged, ISelectorBrowserPageViewModel, ITemplateBrowserPageViewModel where TParameterDefinition : IParameterDefinition
	{
        private record IndexedTemplateParameter(ITemplateParameterViewModel Parameter, int Index);

        private readonly ITemplateParametersService _templateParametersService;
		private IDisposable _refreshSubscription;
		private IBindingSource _context;
		
		public virtual IBindingSource Context
		{
			get
			{
				return this._context;
			}
			set
			{
				if (this._context == value)
				{
					return;
				}
				this._context = value;
				base.RaisePropertyChanged<IBindingSource>(() => this.Context);
				base.RaisePropertyChanged<bool>(() => this.IsVisible);
			}
		}
		public override bool IsVisible
		{
			get
			{
				return this._templateParametersService.IsActivePageTemplatePage && this.Context != null && !(this.Context.SourceInstance is IParameterDefinition);
			}
		}
		protected TemplateBrowserPageViewModel(IUiScheduler uiScheduler, IAsyncScheduler asyncScheduler, ITemplateParametersService templateParametersService) : base(uiScheduler, asyncScheduler)
		{
			Contract.Requires<ArgumentNullException>(templateParametersService != null, "templateParametersService");
			this._templateParametersService = templateParametersService;
		}
		protected override void FillValues(bool invalidateCache)
		{
			if (this._refreshSubscription != null)
			{
				this._refreshSubscription.Dispose();
			}
			base.Values.Clear();
			this.asyncScheduler.Schedule(TimeSpan.FromSeconds(0.5), new Action(this.LoadTemplateParameters));
		}
		private void LoadTemplateParameters()
		{
			IObservable<ITemplateParameterViewModel> templateParameters = this._templateParametersService
				.GetActivePageTemplateParameters<TParameterDefinition>()
				.Select(new Func<TParameterDefinition, ITemplateParameterViewModel>(this.CreateViewModel))
				.ObserveOn(this.uiScheduler);

            this._refreshSubscription = templateParameters
				.Select(SelectWithIndex)
				.Do(ClearValuesForFirstParameter)
				.Select(GetParameterViewModel)
				.Subscribe(
					new Action<ITemplateParameterViewModel>(base.Values.Add),
					new Action<Exception>(base.HandleRefreshError), 
					new Action(this.EndRefresh));
		}
		protected override ITemplateParameterViewModel CreateViewModel(TParameterDefinition itemModel)
		{
			return new TemplateParameterDefinitionViewModel(itemModel);
		}
		
		private static IndexedTemplateParameter SelectWithIndex(ITemplateParameterViewModel parameterViewModel, int index)
		{
			return new IndexedTemplateParameter(parameterViewModel, index);
		}
		
		private void ClearValuesForFirstParameter(IndexedTemplateParameter indexedParameter)
		{
			if (indexedParameter.Index == 0)
			{
				base.Values.Clear();
			}
		}
		
		private static ITemplateParameterViewModel GetParameterViewModel(IndexedTemplateParameter indexedParameter)
		{
			return indexedParameter.Parameter;
		}

	}
}
