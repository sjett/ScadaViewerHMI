// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 层级节点接口
    /// </summary>
    public interface IHierarchicalNode
    {
        /// <summary>
        /// 父节点改变事件
        /// </summary>
        event EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>> ParentChanged;
        /// <summary>
        /// 父节点
        /// </summary>
        IExplorerNodeViewModel Parent
        {
            get;
            set;
        }
        /// <summary>
        /// 父项目页面
        /// </summary>
        IProjectPageViewModel? ParentPage
        {
            get;
        }
        /// <summary>
        /// 父模板页面
        /// </summary>
        ITemplatePageViewModel? ParentTemplatePage
        {
            get;
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        IObservableCollection<IExplorerNodeViewModel>? Children
        {
            get;
        }
        /// <summary>
        /// 是否为指定节点的后代节点
        /// </summary>
        /// <param name="expectedParent"></param>
        /// <returns></returns>
        bool IsDescendantOf(IExplorerNodeViewModel expectedParent);
    }
}
