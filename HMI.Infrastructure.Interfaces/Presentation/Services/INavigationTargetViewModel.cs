// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 导航目标服务接口
    /// </summary>
    public interface INavigationTargetService
    {
        /// <summary>
        /// 导航目标集合
        /// </summary>
        IObservableCollection<INavigationTargetViewModel> NavigationTargets
        {
            get;
        }
        /// <summary>
        /// 更新导航目标
        /// </summary>
        /// <param name="addedItems">要添加的项目</param>
        /// <param name="removedItems">要移除的项目</param>
        void UpdateNavigationTargets(IEnumerable<IExplorerNodeViewModel> addedItems, IEnumerable<IExplorerNodeViewModel> removedItems);
    }

}
