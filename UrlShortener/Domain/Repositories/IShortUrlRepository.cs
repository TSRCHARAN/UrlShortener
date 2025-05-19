using UrlShortener.Domain.Models;
using UrlShortener.Persistence.Repositories;

namespace UrlShortener.Domain.Repositories
{
    public interface IShortUrlRepository : IGenericRepository<ShortUrlDatum>
    {
        public Task<ShortUrlDatum> CreateShortUrl(string originalUrl);
        public Task<ShortUrlDatum> GetOriginalUrlByShortUrlAsync(string shorturl);
    }
}
