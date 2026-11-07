// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
namespace HMI.Common.Collections
{
    /// <summary>
    /// ObservableCollection 扩展方法
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// 添加范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (T current in items)
            {
                collection.Add(current);
            }
        }
        /// <summary>
        /// 移除范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (T current in items)
            {
                collection.Remove(current);
            }
        }
    }
}
