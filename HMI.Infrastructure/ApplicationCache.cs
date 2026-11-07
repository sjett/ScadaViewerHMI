// SPDX-License-Identifier: GPL-3.0-only
using System.Runtime.Caching;

namespace HMI.Infrastructure
{
    /// <summary>
    /// 应用程序缓存
    /// </summary>
    public sealed class ApplicationCache : ObjectCache
    {
        private readonly IDictionary<string, object> cacheItems = new Dictionary<string, object>();
        private readonly IDictionary<string, DateTime> lastAccessTimes = new Dictionary<string, DateTime>();
        private readonly IDictionary<string, CacheItemPolicy> cachePolicies = new Dictionary<string, CacheItemPolicy>();
        private readonly object syncRoot = new();
        private readonly string cacheName;

        public static ApplicationCache Default
        {
            get;
            private set;
        }
        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return DefaultCacheCapabilities.InMemoryProvider | DefaultCacheCapabilities.CacheEntryChangeMonitors | DefaultCacheCapabilities.AbsoluteExpirations | DefaultCacheCapabilities.SlidingExpirations | DefaultCacheCapabilities.CacheEntryUpdateCallback | DefaultCacheCapabilities.CacheEntryRemovedCallback;
            }
        }
        public override string Name
        {
            get
            {
                return cacheName;
            }
        }
        public override object this[string key]
        {
            get
            {
                return Get(key, null);
            }
            set
            {
                Set(key, value, new CacheItemPolicy(), null);
            }
        }
        static ApplicationCache()
        {
            Default = new ApplicationCache("Default");
        }
        private ApplicationCache(string name)
        {
            cacheName = name;
        }
        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null)
        {
            throw new NotImplementedException();
        }
        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            IEnumerator<KeyValuePair<string, object>> result;
            lock (syncRoot)
            {
                List<KeyValuePair<string, object>> list = cacheItems.ToList();
                result = list.GetEnumerator();
            }
            return result;
        }
        public override bool Contains(string key, string regionName = null)
		{
			lock (syncRoot)
            {
				return cacheItems.ContainsKey(key);
			}
		}
        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return AddOrGetExisting(key, value, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            }, null);
        }
        public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
        {
            object obj = this.AddOrGetExisting(value.Key, value.Value, policy, null);
            if (obj != null)
            {
                return new CacheItem(value.Key, obj, value.RegionName);
            }
            return null;
        }
        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            object result;
            lock (syncRoot)
            {
                object existingValue = Get(key, regionName);
                if (existingValue != null)
                {
                    result = existingValue;
                }
                else
                {
                    Set(key, value, policy, regionName);
                    result = null;
                }
            }
            return result;
        }
        public override object Get(string key, string regionName = null)
        {
            object result;
            lock (syncRoot)
            {
                if (cacheItems.TryGetValue(key, out object cachedValue))
                {
                    UpdateLastAccessTime(key);
                    result = cachedValue;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        private void UpdateLastAccessTime(string key)
        {
            lock (syncRoot)
            {
                lastAccessTimes[key] = DateTime.Now;
            }
        }

        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            object obj = Get(key, regionName);
            if (obj != null)
            {
                return new CacheItem(key, obj, regionName);
            }
            return null;
        }
        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            Set(key, value, new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration
            }, regionName);
        }
        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            this.Set(item.Key, item.Value, policy, item.RegionName);
        }
        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            lock (syncRoot)
            {
                cacheItems[key] = value;
                cachePolicies[key] = policy;
                UpdateLastAccessTime(key);
            }
        }
        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            IDictionary<string, object> result;
            lock (syncRoot)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> current in cacheItems)
                {
                    UpdateLastAccessTime(current.Key);
                    dictionary.Add(current.Key, current.Value);
                }
                result = dictionary;
            }
            return result;
        }
        public override object Remove(string key, string regionName = null)
        {
            object result;
            lock (syncRoot)
            {
                object removedValue = Get(key, regionName);
                cacheItems.Remove(key);
                lastAccessTimes.Remove(key);
                cachePolicies.Remove(key);
                result = removedValue;
            }
            return result;
        }
        public override long GetCount(string regionName = null)
		{
			lock (syncRoot)
            {
				return cacheItems.Count;
			}
		}
    }
}