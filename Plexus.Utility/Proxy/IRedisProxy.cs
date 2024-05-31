
namespace Plexus.Utility.Proxy
{
    public interface IRedisProxy
    {
        /// <summary>
        /// Set cache with given key and specify time to live
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheMinute"></param>
        /// <returns></returns>
        bool SetCache<T>(string key, T value, int cacheMinute = 30) where T : class;

        /// <summary>
        /// Get cache data of given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetCache<T>(string key) where T : class;

        /// <summary>
        /// Remove cache of given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ClearCache(string key);

        /// <summary>
        /// Get redis server info
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetServerInfo();
    }
}