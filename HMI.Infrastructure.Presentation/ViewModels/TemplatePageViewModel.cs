// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 模板页视图模型
    /// </summary>
    public class TemplatePageViewModel : ProjectPageViewModel,  ITemplatePageViewModel
    {
        public const string TemplatePageTypeName = "TemplatePage";
        private readonly IObservableCollection<IParameterDefinition> _templateParameters;
        
        protected ICollectionMonitor<IParameterDefinition> TemplateParametersMonitor
        {
            get;
            set;
        }

        [CollectionEditor(typeof(SignalParameterDefinition), AvailableTypes = new Type[]
		{
			typeof(SymbolicTextParameterDefinition),
			typeof(AlarmGroupParameterDefinition),
			typeof(AlarmTypeParameterDefinition)
		}, IsGroupingEnabled = true, ExpandAllGroups = true, UseHorizontalEditing = true), PropertyCategory("Template", typeof(StandardPropertyCategories))]
        public IObservableCollection<IParameterDefinition> TemplateParameters
        {
            get
            {
                return _templateParameters;
            }
        }
        public TemplatePageViewModel(IServiceProvider container, IExplorerNodeViewModel parentNode, string name)
            : base(container, parentNode, name)
        {
            _templateParameters = new FullyObservableCollection<IParameterDefinition>();
            TemplateParametersMonitor = container.Resolve<ICollectionMonitor<IParameterDefinition>>();
            TemplateParametersMonitor.Start(TemplateParameters);
        }
    }
}
