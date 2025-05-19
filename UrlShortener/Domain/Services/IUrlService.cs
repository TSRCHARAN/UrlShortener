using UrlShortener.Domain.Services.Communication;

namespace UrlShortener.Domain.Services
{
    public interface IUrlService
    {
        public Task<ServiceResult> CreateUrl(string originalUrl);
        public Task<ServiceResult> GetOriginalUrl(string shortUrl);
    }
}
