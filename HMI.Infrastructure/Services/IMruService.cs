// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 最近使用的项目文件服务接口
    /// </summary>
    public interface IMruService
    {
        /// <summary>
        /// 将项目文件添加到最近使用的文件列表顶部
        /// </summary>
        /// <param name="projectFile">项目文件</param>
        void PrependProjectToMRU(string projectFile);
        /// <summary>
        /// 获取最近使用的项目文件列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<MruEntry> GetMruItems();
    }
}
