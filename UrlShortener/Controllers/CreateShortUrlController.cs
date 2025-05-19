using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrlShortener.Domain.Services;
using UrlShortener.Domain.Services.Communication;
using UrlShortener.Services;
using UrlShortener.ViewModel;

namespace UrlShortener.Controllers
{
    public class CreateShortUrlController : Controller
    {
        private readonly IUrlService _urlService;

        public CreateShortUrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateShortUrlByOriginalUrl(string originalUrl)
        {
            var response = await _urlService.CreateUrl(originalUrl);
            if (response == null || !response.Success)
            {
                AlertViewModel alert = new AlertViewModel { Message = response?.Message ?? "Invalid URL. Please try again." };
                TempData["Alert"] = JsonConvert.SerializeObject(alert);
                return Json(new { response });
            }
            else
            {
                AlertViewModel alert = new AlertViewModel { IsSuccess = true, Message = response.Message };
                TempData["Alert"] = JsonConvert.SerializeObject(alert);
                return Json(new { response });
            }
        }

    }
}
