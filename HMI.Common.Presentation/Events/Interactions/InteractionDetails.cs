// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
    /// <summary>
    /// 交互详情基类
    /// </summary>
    public class InteractionDetails
	{
        /// <summary>
        /// 交互窗口是否为活动窗口的子窗口
        /// </summary>
        public bool IsActiveWindowChild { get; set; }
        /// <summary>
        /// 交互详情基类
        /// </summary>
		public InteractionDetails()
		{
			this.IsActiveWindowChild = true;
		}
	}
}
