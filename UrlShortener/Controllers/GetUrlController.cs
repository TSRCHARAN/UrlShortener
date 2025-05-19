using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UrlShortener.Domain.Services;
using UrlShortener.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UrlShortener.Controllers
{
    public class GetUrlController : Controller
    {
        private readonly IUrlService _urlService;

        public GetUrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetOriginalUrlByShortUrl(string shortUrl)
        {
            var response = await _urlService.GetOriginalUrl(shortUrl);
            if (response == null || !response.Success)
            {
                AlertViewModel alert = new AlertViewModel { Message = response.Message };
                TempData["Alert"] = JsonConvert.SerializeObject(alert);
                return Json(new { response });
            }
            else
            {
                AlertViewModel alert = new AlertViewModel { IsSuccess = true, Message = response.Message };
                TempData["Alert"] = JsonConvert.SerializeObject(alert);
                var resource = response.Resource;
                var s = Json(new { resource });
                return s;
            }
        }
    }
}
