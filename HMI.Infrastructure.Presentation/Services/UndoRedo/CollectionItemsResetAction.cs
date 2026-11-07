// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;

namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 集合项重置操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionItemsResetAction<T> : ActionBase<T>
	{
		private readonly T[] olditems;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStartingIndex"></param>
        /// <param name="newStartingIndex"></param>
        /// <param name="oldItems"></param>
        public CollectionItemsResetAction(IObservableCollection<T> target, int oldStartingIndex, int newStartingIndex, T[] oldItems) : base(target, oldStartingIndex, newStartingIndex)
		{
			this.olditems = (oldItems ?? new T[0]);
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public override void Undo()
		{
			this.Items.BeginEdit();
			this.Items.Clear();
			T[] array = this.olditems;
			for (int i = 0; i < array.Length; i++)
			{
				T item = array[i];
				this.Items.Add(item);
			}
			this.Items.EndEdit();
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public override void Redo()
		{
			this.Items.Clear();
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return "Collection reset";
		}
	}
}
