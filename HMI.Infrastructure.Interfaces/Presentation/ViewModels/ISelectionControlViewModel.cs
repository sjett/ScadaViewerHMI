// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 选择控件视图模型接口
    /// </summary>
    public interface ISelectionControlViewModel : ICommonControlViewModel
    {
        /// <summary>
        /// 更新组合边界
        /// </summary>
        void UpdateGroupBounds();
        /// <summary>
        /// 从选择创建组合插件视图模型
        /// </summary>
        /// <returns></returns>
        ICommonControlViewModel CreateGroupFromSelection();
    }
}
