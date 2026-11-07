// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Domain.Models
{
    /// <summary>
    /// 页面接口
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// 是否使用页面名称作为信号前缀
        /// </summary>
        bool UsePageNameAsSignalPrefix
        {
            get;
        }
        /// <summary>
        /// ID
        /// </summary>
        string ID
        {
            get;
        }
        /// <summary>
        /// 信号前缀
        /// </summary>
        string SignalPrefix
        {
            get;
        }
    }
}
