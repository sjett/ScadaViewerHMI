// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 集合项替换操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionItemsReplacedAction<T> : ActionBase<T>
	{
		private readonly T[] addedItems;
		private readonly T[] removedItems;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        /// <param name="addedItems"></param>
        /// <param name="removedItems"></param>
        public CollectionItemsReplacedAction(
			IObservableCollection<T> target, 
			int oldStartingIndex, 
			int newStartingIndex, 
			T[] addedItems, 
			T[] removedItems): base(target, oldStartingIndex, newStartingIndex)
		{
			this.addedItems = addedItems;
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
				T value = array[i];
				this.Items[int_++] = value;
			}
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public override void Redo()
		{
			int int_ = this.OldStartingIndex;
			T[] array = this.addedItems;
			for (int i = 0; i < array.Length; i++)
			{
				T value = array[i];
				this.Items[int_++] = value;
			}
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return "Collection items replaced";
		}
	}
}
