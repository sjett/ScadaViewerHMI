// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 模板页面视图模型接口
    /// </summary>
    public interface ITemplatePageViewModel : IProjectPageViewModel
    {
        /// <summary>
        /// 模板参数集合
        /// </summary>
        IObservableCollection<IParameterDefinition> TemplateParameters
        {
            get;
        }
    }
}
