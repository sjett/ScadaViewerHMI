// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 操作监视器接口
    /// </summary>
    public interface IActionMonitor
    {
        /// <summary>
        /// 是否可以重做
        /// </summary>
        bool CanRedo
        {
            get;
        }
        /// <summary>
        /// 是否可以撤销
        /// </summary>
        bool CanUndo
        {
            get;
        }
        /// <summary>
        /// 取消当前操作
        /// </summary>
        void CancelAction();
        /// <summary>
        /// 重做操作
        /// </summary>
        void Redo();
        /// <summary>
        /// 撤销操作
        /// </summary>
        void Undo();
        /// <summary>
        /// 重置操作监视器
        /// </summary>
        void Reset();
        /// <summary>
        /// 记录操作
        /// </summary>
        /// <param name="action"></param>
        void RecordAction(IAction action);
        /// <summary>
        /// 开始组合操作
        /// </summary>
        void StartGrouping();
        /// <summary>
        /// 结束组合操作
        /// </summary>
        void EndGrouping();
        /// <summary>
        /// 禁用操作监视器
        /// </summary>
        void Disable();
    }


}
