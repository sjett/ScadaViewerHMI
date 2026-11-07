// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 属性监视器接口
    /// </summary>
    public interface IPropertyMonitor
    {
        /// <summary>
        /// 开始监视
        /// </summary>
        /// <param name="target"></param>
        void Start(INotifyProperty target);
        /// <summary>
        /// 停止监视
        /// </summary>
        void Stop();
    }

}
