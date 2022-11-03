using System;
using System.Collections.Generic;

namespace AMS.JiraServicedesk.Cache
{
    public interface ICache
    {
        object this[string key] { get; }

        void InsertNonExpiration(string key, object cacheObject);

        void InsertAbsoluteCache(string key, object cacheObject, TimeSpan cacheTime);
        void InsertSlidingCache(string key, object cacheObject, TimeSpan cacheTime);

        void InsertAbsoluteRangeCache(Dictionary<string, object> keyValues, TimeSpan cacheTime);
        void InsertSlidingRangeCache(Dictionary<string, object> keyValues, TimeSpan cacheTime);
        void InsertRangeCache(Dictionary<string, object> keyValues);

        void RemoveCache(string key);
    }
}
