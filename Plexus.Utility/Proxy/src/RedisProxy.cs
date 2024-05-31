using System;
using ServiceStack.Redis;

namespace Plexus.Utility.Proxy.src
{
    public class RedisProxy : IRedisProxy
    {
        private readonly IRedisClientsManager _clientManger;

        public RedisProxy(IRedisClientsManager clientManager)
        {
            _clientManger = clientManager;
        }

        public T GetCache<T>(string key) where T : class
        {
            using (var cache = _clientManger.GetClient())
            {
                return cache.Get<T>(key);
            }
        }

        public bool SetCache<T>(string key, T value, int cacheMinute = 30) where T : class
        {
            var expireSpan = new TimeSpan(0, cacheMinute, 0);

            using (var cache = _clientManger.GetClient())
            {
                return cache.Set(key, value, expireSpan);
            }
        }

        public bool ClearCache(string key)
        {
            using (var cache = _clientManger.GetClient())
            {
                return cache.Remove(key);
            }
        }

        public Dictionary<string, string> GetServerInfo()
        {
            using (var cache = _clientManger.GetClient())
            {
                return cache.Info;
            }
        }
    }
}

