// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 缓存条目
    /// </summary>
    /// <typeparam name="TValue">缓存数据类型</typeparam>
    public class CacheEntry<TValue>
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        public TValue Value
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">缓存数据</param>
        public CacheEntry(TValue value)
        {
            Value = value;
        }
    }
}
