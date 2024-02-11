using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Helios.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            if (this.HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                return RedirectToAction("Me", "Me");
            }

            ViewBag.Error = null;//= "no_password";
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}