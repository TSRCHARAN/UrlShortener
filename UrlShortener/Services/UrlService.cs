using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Linq.Expressions;
using UrlShortener.Domain.Models;
using UrlShortener.Domain.Repositories;
using UrlShortener.Domain.Services;
using UrlShortener.Domain.Services.Communication;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _redisCacheService;
        public UrlService(IUnitOfWork unitOfWork, IRedisCacheService redisCacheService)
        {
            _unitOfWork = unitOfWork;
            _redisCacheService = redisCacheService;
        }
        public async Task<ServiceResult> CreateUrl(string originalUrl)
        {
            int count = 0;
            do
            {
                var tempShortUtl = GenerateRandomUrl();
                var data = new ShortUrlDatum()
                {
                    ShortUrl = tempShortUtl,
                    OriginalUrl = originalUrl,
                    CreatedBy = "system",
                    CreatedDate = DateTime.Now
                };
                var response = await _unitOfWork.ShortUrlRepository.AddAsync(data);

                var json = JsonConvert.SerializeObject(data);
                await _redisCacheService.SetCacheAsync(tempShortUtl, json, TimeSpan.FromMinutes(10));

                if (response)
                {
                    return new ServiceResult(true, "URL Created Successfully", tempShortUtl);
                }

                count++;

            } while (count < 3);

            return new ServiceResult(false, "URL Creation Failed! Please resubmit.");
        }
        public async Task<ServiceResult> GetOriginalUrl(string shortUrl)
        {
            try
            {
                var tempData = await _redisCacheService.GetCacheAsync(shortUrl);
                if (tempData != null)
                {
                    var json = JsonConvert.DeserializeObject<ShortUrlDatum>(tempData.ToString());
                    return new ServiceResult(true, "Found Original Url Successfully", json.OriginalUrl);
                }

                var data = _unitOfWork.ShortUrlRepository.GetOriginalUrlByShortUrlAsync(shortUrl);
                if (data.Result == null)
                {
                    return new ServiceResult(false, "No such Url found");
                }
                return new ServiceResult(true, "Found Original Url Successfully", data.Result.OriginalUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResult(false, "No such Url found");
            }
        }
        public string GenerateRandomUrl()
        {
            var shortUrl = "";

            for (int i = 0; i < 6; i++)
            {
                Random random = new Random();

                // Generate a random character
                char randomChar = GetRandomChar(random);

                shortUrl += randomChar;
            }
            return shortUrl;
        }
        public char GetRandomChar(Random random)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            int index = random.Next(chars.Length);

            return chars[index];
        }
    }
}
