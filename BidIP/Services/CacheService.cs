using Microsoft.Extensions.Caching.Memory;

namespace BidIP.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SetUserData(string key, List<string[]> data)
        {
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(10)); // 缓存10分钟
        }

        public List<string[]> GetUserData(string key)
        {
            return _memoryCache.TryGetValue(key, out List<string[]> data) ? data : null;
        }
    }
}
