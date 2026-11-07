// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Domain.Models
{
    /// <summary>
    /// 绑定源接口
    /// </summary>
    public interface IBindingSource
    {
        /// <summary>
        /// 属性路径
        /// </summary>
        string PropertyPath
        {
            get;
        }
        /// <summary>
        /// 源实例
        /// </summary>
        object SourceInstance
        {
            get;
        }
    }
}
