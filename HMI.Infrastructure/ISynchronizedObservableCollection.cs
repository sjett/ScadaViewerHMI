// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure
{
    /// <summary>
    /// 同步可观察集合接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISynchronizedObservableCollection<T> : IObservableCollection<T>
    {
        /// <summary>
        /// 同步根对象
        /// </summary>
        object SyncRoot
        {
            get;
        }
    }
}
