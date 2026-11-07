// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 表示一个可观察的集合组
    /// </summary>
    public interface IObservableCollectionGroup : IObservableCollection<IExplorerNodeViewModel>, IDisposable
    {
        /// <summary>
        /// 添加非组项
        /// </summary>
        /// <param name="item"></param>
        void AddNonGroupItem(IExplorerNodeViewModel item);
        /// <summary>
        /// 移除非组项
        /// </summary>
        /// <param name="item"></param>
        void RemoveNonGroupItem(IExplorerNodeViewModel item);
    }
}
