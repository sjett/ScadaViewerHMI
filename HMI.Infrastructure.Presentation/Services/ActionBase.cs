// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Services;

namespace HMI.Infrastructure.Presentation.Services
{
    
    /// <summary>
    /// 操作基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ActionBase<T> : IAction
    {
        /// <summary>
        /// 目标集合
        /// </summary>
        protected readonly IObservableCollection<T> Items;
        /// <summary>
        /// 新起始索引
        /// </summary>
        protected readonly int NewStartingIndex;
        /// <summary>
        /// 旧起始索引
        /// </summary>
        protected readonly int OldStartingIndex;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        public ActionBase(IObservableCollection<T> target, int oldStartingIndex, int newStartingIndex)
        {
            Items = target;
            OldStartingIndex = oldStartingIndex;
            NewStartingIndex = newStartingIndex;
        }
        /// <summary>
        /// 与下一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual IAction GroupWith(IAction nextAction)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 是否可以与下一个操作组合
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual bool CanGroup(IAction action)
        {
            return false;
        }
        /// <summary>
        /// 撤销操作
        /// </summary>
        public abstract void Undo();
        /// <summary>
        /// 重做操作
        /// </summary>
        public abstract void Redo();
    }

}
