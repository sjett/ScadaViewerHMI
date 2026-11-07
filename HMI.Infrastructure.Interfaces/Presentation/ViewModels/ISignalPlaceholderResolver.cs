// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 信号占位符解析器接口
    /// </summary>
    public interface ISignalPlaceholderResolver
    {
        /// <summary>
        /// 获取完整的信号名称
        /// </summary>
        /// <param name="signalSuffix"></param>
        /// <returns></returns>
        string GetFullSignalName(string signalSuffix);
        /// <summary>
        /// 获取信号后缀
        /// </summary>
        /// <param name="fullSignalName"></param>
        /// <returns></returns>
        string GetSignalSuffix(string fullSignalName);
        /// <summary>
        /// 获取信号前缀
        /// </summary>
        /// <returns></returns>
        string GetSignalPrefix();
    }
}
