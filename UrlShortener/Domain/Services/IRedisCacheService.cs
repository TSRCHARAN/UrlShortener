using System;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Services
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync(string key, string value, TimeSpan? expiry = null);
        Task RemoveCacheAsync(string key);
        Task<string?> GetCacheAsync(string key);
    }
}
