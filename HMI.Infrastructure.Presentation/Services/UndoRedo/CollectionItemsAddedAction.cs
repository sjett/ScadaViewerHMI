// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 集合添加项操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionItemsAddedAction<T> : ActionBase<T>
	{
		private readonly T[] newItems;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        /// <param name="newItems"></param>
        public CollectionItemsAddedAction(IObservableCollection<T> target, int oldStartingIndex, int newStartingIndex, T[] newItems) : base(target, oldStartingIndex, newStartingIndex)
		{
			this.newItems = newItems;
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public override void Undo()
		{
			this.Items.RemoveAt(this.NewStartingIndex);
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public override void Redo()
		{
			int int_ = this.NewStartingIndex;
			T[] array = this.newItems;
			for (int i = 0; i < array.Length; i++)
			{
				T item = array[i];
				this.Items.Insert(int_++, item);
			}
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return "Collection items added";
		}
	}
}
