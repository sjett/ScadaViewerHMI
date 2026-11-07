// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{

    /// <summary>
    /// 操作接口
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 与下一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        IAction GroupWith(IAction nextAction);
        /// <summary>
        /// 是否可以与下一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        bool CanGroup(IAction nextAction);
        /// <summary>
        /// 撤销操作
        /// </summary>
        void Undo();
        /// <summary>
        /// 重做操作
        /// </summary>
        void Redo();
    }

}
