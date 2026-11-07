// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Collections
{
    /// <summary>
    /// 可观察栈实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableStack<T> : FullyObservableCollection<T>, IObservableStack<T>
    {
        /// <summary>
        /// 入栈
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            base.Insert(0, value);
        }
        /// <summary>
        /// 出栈
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T result = this.Peek();
            base.RemoveAt(0);
            return result;
        }
        /// <summary>
        /// 查看栈顶元素
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return base[0];
        }
    }
}
