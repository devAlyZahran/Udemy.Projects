using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialMediaLinks.Models;

namespace SocialMediaLinks.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _options;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IOptions<SocialMediaLinksOptions> options, IWebHostEnvironment webHostEnvironment)
        {
            _options = options.Value;
            _webHostEnvironment = webHostEnvironment;
        }


        [Route("/")]
        public IActionResult Index()
        {
            @ViewBag.Facebook = _options.Facebook;
            @ViewBag.Twitter = _options.Twitter;
            @ViewBag.Youtube = _options.Youtube;

            if (!_webHostEnvironment.IsDevelopment())
            {
                @ViewBag.Instagram = _options.Instagram;
            }

            return View();
        }
    }
}
