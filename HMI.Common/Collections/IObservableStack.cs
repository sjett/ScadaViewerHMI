// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Collections
{
    /// <summary>
    /// 可观察栈接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableStack<T> : IList<T>, IObservableCollection<T>
    {
        /// <summary>
        /// 入栈
        /// </summary>
        /// <param name="value"></param>
        void Push(T value);
        /// <summary>
        /// 出栈
        /// </summary>
        /// <returns></returns>
        T Pop();
        /// <summary>
        /// 查看栈顶元素
        /// </summary>
        /// <returns></returns>
        T Peek();
    }
}
