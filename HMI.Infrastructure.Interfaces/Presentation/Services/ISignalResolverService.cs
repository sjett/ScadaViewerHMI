// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Linq.Expressions;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 信号解析服务
    /// </summary>
    public interface ISignalResolverService
    {
        /// <summary>
        /// 获取解析后的信号前缀
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="activePage"></param>
        /// <returns></returns>
        string? GetResolvedSignalPrefix(object sourceObject, IPage activePage);
        /// <summary>
        /// 获取信号监视器
        /// </summary>
        /// <param name="model"></param>
        /// <param name="signal"></param>
        /// <param name="resolvedSignal"></param>
        /// <returns></returns>
        ISignalWatcher GetSignalWatcher(ICommonControlViewModel model, Expression<Func<string>> signal, Expression<Func<string>> resolvedSignal);
    }
}
