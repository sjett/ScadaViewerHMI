// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Reactive.Linq;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 模板参数服务
    /// </summary>
    public class TemplateParametersService : ITemplateParametersService
    {
        private readonly IProjectManagerViewModel _projectManager;
        public bool IsActivePageTemplatePage
        {
            get
            {
                return _projectManager.ActivePage is ITemplatePageViewModel;
            }
        }
        public TemplateParametersService(IProjectManagerViewModel solution)
        {
            _projectManager = solution;
        }
        public IObservable<T> GetActivePageTemplateParameters<T>()
        {
            ITemplatePageViewModel templatePageViewModel = _projectManager.ActivePage as ITemplatePageViewModel;
            if (templatePageViewModel == null)
            {
                return Observable.Empty<T>(); 
            }
            return templatePageViewModel.TemplateParameters.OfType<T>().ToObservable<T>();
        }
    }
}
