// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Domain.Models
{
    /// <summary>
    /// 解决方案接口
    /// </summary>
    public interface ISolution
    {
        /// <summary>
        /// 项目
        /// </summary>
        IProject Project
        {
            get;
        }
        /// <summary>
        /// 默认活动页面
        /// </summary>
        IPage ActivePage
        {
            get;
        }
    }
}
