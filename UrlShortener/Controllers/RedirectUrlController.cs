using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrlShortener.Domain.Services;
using UrlShortener.ViewModel;

namespace UrlShortener.Controllers
{
    //[ApiController]
    public class RedirectUrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public RedirectUrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        [Route("api/{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrlByShortUrl(string shortUrl)
        {
            var response = await _urlService.GetOriginalUrl(shortUrl);

            if (response == null || !response.Success)
            {
                return NotFound("Incorrect URL");
            }
            var url = response.Resource.ToString();
            return Redirect(url);
        }
    }
}
