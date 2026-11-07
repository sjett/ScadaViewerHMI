// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 弃用：可分组接口
    /// </summary>
    [Obsolete]
    public interface IGroupable
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        Guid ID
        {
            get;
        }
        /// <summary>
        /// 父级唯一标识符
        /// </summary>
        Guid ParentID
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为组
        /// </summary>
        bool IsGroup
        {
            get;
            set;
        }
    }
}
