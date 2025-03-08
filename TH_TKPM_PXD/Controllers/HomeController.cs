using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using ASC.Utilities;
using TH_TKPM_PXD.Configuration;
using TH_TKPM_PXD.Models;
using Microsoft.AspNetCore.Http;

namespace ASC.Web.Controllers
{
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;
        private IOptions<ApplicationSettings> _settings;

        public HomeController(
            IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public IActionResult Index()
        {
            // Thiết lập Session
            HttpContext.Session.SetSession("Test", _settings.Value);

            // Lấy Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");

            // Sử dụng IOptions
            ViewBag.Title = settings.ApplicationTitle;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
