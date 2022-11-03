using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace AMS.JiraServicedesk.Cache
{
    public class MyMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public MyMemoryCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public object this[string key]
        {
            get
            {
                object cacheObject;

                // Look for cache key.
                if (!_cache.TryGetValue(key, out cacheObject))
                {
                    return null;
                }

                return cacheObject;
            }
        }

        public void InsertNonExpiration(string key, object cacheObject)
        {
            //NOTE: Memory pressure. If the system is resource-constrained, and a running app needs additional memory, cached items are eligible to be removed from memory to free up RAM. You can however disable this by setting the cache priority for the entry to CacheItemPriority.NeverRemove (here we do not need to make it complicated).
            _cache.Set(key, cacheObject);
        }

        public void InsertAbsoluteCache(string key, object cacheObject, TimeSpan cacheTime)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time.
                .SetAbsoluteExpiration(cacheTime);

            // Save data in cache.
            _cache.Set(key, cacheObject, cacheEntryOptions);
        }

        public void InsertSlidingCache(string key, object cacheObject, TimeSpan cacheTime)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(cacheTime);

            // Save data in cache.
            _cache.Set(key, cacheObject, cacheEntryOptions);
        }

        public void InsertAbsoluteRangeCache(Dictionary<string, object> keyValues, TimeSpan cacheTime)
        {
            foreach (var keyValue in keyValues)
            {
                InsertAbsoluteCache(keyValue.Key, keyValue.Value, cacheTime);
            }
        }

        public void InsertSlidingRangeCache(Dictionary<string, object> keyValues, TimeSpan cacheTime)
        {
            foreach (var keyValue in keyValues)
            {
                InsertSlidingCache(keyValue.Key, keyValue.Value, cacheTime);
            }
        }

        public void InsertRangeCache(Dictionary<string, object> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                InsertNonExpiration(keyValue.Key, keyValue.Value);
            }
        }

        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }
    }
}