// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Collections
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            using (IEnumerator<T> enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    T item = enumerator.Current;
                    collection.Add(item);
                }
            }
        }
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            using (IEnumerator<T> enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    collection.Remove(current);
                }
            }
        }
    }
}
