using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSKBooks1.Models;

namespace SSKBooks1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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

        public IActionResult Error404()
        {
            return View("~/Views/Shared/Error404.cshtml");
        }

        public IActionResult Error500()
        {
            return View("~/Views/Shared/Error500.cshtml");
        }
    }
}
