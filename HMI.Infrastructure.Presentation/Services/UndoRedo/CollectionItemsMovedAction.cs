// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 集合项移动操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionItemsMovedAction<T> : ActionBase<T>
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        public CollectionItemsMovedAction(IObservableCollection<T> target, int oldStartingIndex, int newStartingIndex) : base(target, oldStartingIndex, newStartingIndex)
		{
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public override void Undo()
		{
			this.Items.Move(this.NewStartingIndex, this.OldStartingIndex);
		}
        /// <summary>
        /// 重做操作
        /// </summary>
		public override void Redo()
		{
			this.Items.Move(this.OldStartingIndex, this.NewStartingIndex);
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return "Collection items moved";
		}
	}
}
