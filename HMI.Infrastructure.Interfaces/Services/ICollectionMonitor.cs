// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 集合监视器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionMonitor<T>
    {
        /// <summary>
        /// 开始监视
        /// </summary>
        /// <param name="target"></param>
        void Start(IObservableCollection<T> target);
        /// <summary>
        /// 停止监视
        /// </summary>
        void Stop();
    }


}
