using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Models;
using UrlShortener.Domain.Repositories;
using UrlShortener.Persistence.Repositories;

namespace UrlShortener.Persistance.Repositories
{
    public class ShortUrlRepository : GenericRepository<ShortUrlDatum, UrlDbContext>,
        IShortUrlRepository
    {
        private readonly ILogger _logger;
        public ShortUrlRepository(UrlDbContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
        }

        public async Task<ShortUrlDatum> CreateShortUrl(string originalUrl)
        {
            try
            {
                return await Context.ShortUrlData.FirstOrDefaultAsync(db => db.OriginalUrl == originalUrl);
            }
            catch (Exception error)
            {
                _logger.LogError("Get DatabaseConfigByName Exception::Database exception: {0}", error);
                return null;

            }
        }

        public async Task<ShortUrlDatum> GetOriginalUrlByShortUrlAsync(string shorturl)
        {
            try
            {
                return await Context.ShortUrlData.FirstOrDefaultAsync(db => db.ShortUrl == shorturl);
            }
            catch (Exception error)
            {
                _logger.LogError("Get DatabaseConfigByName Exception::Database exception: {0}", error);
                return null;

            }
        }
    }
}
