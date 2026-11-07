// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 集合项移除操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionItemsRemovedAction<T> : ActionBase<T>
	{
		private readonly T[] removedItems;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        /// <param name="removedItems"></param>
        public CollectionItemsRemovedAction(IObservableCollection<T> target, int oldStartingIndex, int newStartingIndex, T[] removedItems) : base(target, oldStartingIndex, newStartingIndex)
		{
			this.removedItems = removedItems;
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public override void Undo()
		{
			int int_ = this.OldStartingIndex;
			T[] array = this.removedItems;
			for (int i = 0; i < array.Length; i++)
			{
				T item = array[i];
				this.Items.Insert(int_++, item);
			}
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public override void Redo()
		{
			int len = this.removedItems.Length;
			int len2 = 0;
			while (len2 < len)
			{
				this.Items.RemoveAt(this.OldStartingIndex);
				len2 = len2 + 1;
			}
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return "Collection items removed";
		}
	}
}
