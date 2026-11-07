// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.Specialized;
using System.ComponentModel;

namespace HMI.Common.Collections
{
    /// <summary>
    /// 可观察集合接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableCollection<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// 移动元素
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        void Move(int oldIndex, int newIndex);
        /// <summary>
        /// 开始编辑
        /// </summary>
        void BeginEdit();
        /// <summary>
        /// 结束编辑
        /// </summary>
        void EndEdit();
    }
}
