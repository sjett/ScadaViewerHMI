// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 资源管理器节点视图模型接口
    /// </summary>
    public interface IExplorerNodeViewModel : INotifyProperty, IViewModel, IHierarchicalNode, IDisposable
    {
        /// <summary>
        /// 节点唯一标识符
        /// </summary>
        string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 项目视图模型
        /// </summary>
        IProjectViewModel Project
        {
            get;
        }
        /// <summary>
        /// 是否展开
        /// </summary>
        bool IsExpanded
        {
            get;
            set;
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        bool IsEditable
        {
            get;
        }
        /// <summary>
        /// 节点路径
        /// </summary>
        string Path
        {
            get;
        }
        /// <summary>
        /// 验证是否为有效的拖放目标
        /// </summary>
        /// <param name="payload">负载</param>
        /// <returns></returns>
        bool IsValidDropTarget(IEnumerable<object> payload);
    }
}
