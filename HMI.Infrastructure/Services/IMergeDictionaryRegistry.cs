// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 合并资源字典接口
    /// </summary>
    public interface IMergeDictionaryRegistry
    {
        /// <summary>
        /// 添加资源字典
        /// </summary>
        /// <param name="packUri">资源路径</param>
        void AddDictionaryResource(Uri packUri);
    }
    
}
