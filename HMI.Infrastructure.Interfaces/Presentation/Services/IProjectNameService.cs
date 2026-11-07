// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 项目名称服务接口
    /// </summary>
    public interface IProjectNameService
    {
        /// <summary>
        /// 名称是否合法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsValidName(string name);
        /// <summary>
        /// 名称是否已被使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        bool IsNameInUse(string name, IExplorerNodeViewModel node);
        /// <summary>
        /// 重置名称数据库
        /// </summary>
        void ResetNameDatabase();
        /// <summary>
        /// 注册并获取唯一名称
        /// </summary>
        /// <param name="explorerNodeViewModel"></param>
        /// <param name="proposedName"></param>
        /// <returns></returns>
        string RegisterAndGetUniqueName(IExplorerNodeViewModel explorerNodeViewModel, string proposedName);
        /// <summary>
        /// 移除名称
        /// </summary>
        /// <param name="removedItems"></param>
        void RemoveNames(IEnumerable<IExplorerNodeViewModel> removedItems);
    }

}
