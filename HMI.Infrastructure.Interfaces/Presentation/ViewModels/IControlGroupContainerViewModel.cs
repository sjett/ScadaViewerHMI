// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 控件组容器视图模型接口
    /// </summary>
    public interface IControlGroupContainerViewModel : ICommonControlViewModel
    {
        /// <summary>
        /// 当子节点变化时，是否更新边界
        /// </summary>
        bool UpdateBoundsWhenChildrenChange
        {
            get;
            set;
        }
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="children">子节点</param>
        void AddChildren(IEnumerable<IExplorerNodeViewModel> children);
    }
}
