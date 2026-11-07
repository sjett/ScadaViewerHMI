// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 导航目标视图模型接口
    /// </summary>
    public interface INavigationTargetViewModel : ICommonControlViewModel
    {
        /// <summary>
        /// 是否为有效目标
        /// </summary>
        bool IsInvalidTarget
        {
            get;
        }
    }
}
