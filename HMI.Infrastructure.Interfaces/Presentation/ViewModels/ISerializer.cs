// SPDX-License-Identifier: GPL-3.0-only
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 序列化器接口
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source">实例对象</param>
        /// <returns>节点 JObject</returns>
        JObject? Serialize(object source);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="ancestorNode">上级节点</param>
        /// <param name="root">节点 JObject</param>
        /// <param name="version">版本</param>
        /// <returns>实例对象</returns>
        object Deserialize(IExplorerNodeViewModel ancestorNode, JObject root, Version version);
    }
}
