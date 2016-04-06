namespace PH.Well.Common
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Contracts;

    public class HttpContextCache : ICache
    {
        private static readonly object Sync = new object();

        public T Get<T>(string key, string username) where T : class
        {
            var cacheKey = key + username;

            if (!string.IsNullOrWhiteSpace(cacheKey) && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (T)HttpContext.Current.Cache[cacheKey];
            }

            return null;
        }

        public void AddItem(string key, string username, object objectToCache, int minutesToCacheFor, bool removeIfExists = false)
        {
            var cacheKey = key + username;

            if (!string.IsNullOrWhiteSpace(cacheKey) && objectToCache != null)
            {
                if (removeIfExists)
                {
                    this.RemoveItem(key, username);
                }

                lock (Sync)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, objectToCache, null, DateTime.UtcNow.AddMinutes(minutesToCacheFor), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }

        public void RemoveItem(string key, string username)
        {
            var cacheKey = key + username;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }
    }
}
