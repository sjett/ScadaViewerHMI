// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.Concurrent;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 缓存服务, 简单字典实现
    /// </summary>
    /// <typeparam name="TKey">缓存键类型</typeparam>
    /// <typeparam name="TValue">缓存值类型</typeparam>
    public class CacheService<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, CacheEntry<TValue>> cacheDict = new ConcurrentDictionary<TKey, CacheEntry<TValue>>();
        /// <summary>
        /// 获取或缓存条目
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="resolveUncachedEntryCallback">未缓存条目解析回调</param>
        /// <returns>缓存值</returns>
        public TValue GetOrCacheEntry(TKey key, Func<TKey, TValue> resolveUncachedEntryCallback)
        {
            return cacheDict.GetOrAdd(key, T => resolveValue(T, resolveUncachedEntryCallback)).Value;
        }
        /// <summary>
        /// 解析缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="resolveUncachedEntryCallback">未缓存条目解析回调</param>
        /// <returns>缓存条目</returns>
        private CacheEntry<TValue> resolveValue(TKey key, Func<TKey, TValue> resolveUncachedEntryCallback)
        {
            TValue value = resolveUncachedEntryCallback(key);
            return new CacheEntry<TValue>(value);
        }
        /// <summary>
        /// 移除缓存条目
        /// </summary>
        /// <param name="key">缓存键</param>
        public void RemoveEntry(TKey key)
        {
            CacheEntry<TValue> cacheEntry;
            cacheDict.TryRemove(key, out cacheEntry);
        }
    }
}