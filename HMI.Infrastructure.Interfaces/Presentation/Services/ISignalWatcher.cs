// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 信号监视器接口
    /// </summary>
    public interface ISignalWatcher : IDisposable
    {
        /// <summary>
        /// 启动监视器
        /// </summary>
        void Start();
        /// <summary>
        /// 停止监视器
        /// </summary>
        void Stop();
    }
}
