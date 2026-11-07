// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 选择容器接口
    /// </summary>
    public interface ISelectionContainer
    {
        /// <summary>
        /// 设置选择项
        /// </summary>
        /// <param name="node"></param>
        void SetSelection(ICommonControlViewModel node);
        /// <summary>
        /// 添加到选择项
        /// </summary>
        /// <param name="node"></param>
        void AddToSelection(ICommonControlViewModel node);
        /// <summary>
        /// 从选择项移除
        /// </summary>
        /// <param name="node"></param>
        void RemoveFromSelection(ICommonControlViewModel node);
        /// <summary>
        /// 更新选择项
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isControlPressed"></param>
        void UpdateSelection(ICommonControlViewModel item, bool isControlPressed);
        /// <summary>
        /// 更新容器选择状态
        /// </summary>
        void UpdateContainerSelectionStatus();
        /// <summary>
        /// 重新计算选择边界
        /// </summary>
        void RecalculateSelectionBounds();
        /// <summary>
        /// 选择多个项
        /// </summary>
        /// <param name="items"></param>
        void SelectItems(ICollection<ICommonControlViewModel> items);
    }
}
