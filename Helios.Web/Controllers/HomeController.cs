using Helios.Storage;
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
        private readonly StorageContext _ctx;

        public HomeController(ILogger<HomeController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/")]
        public IActionResult Index()
        {
            if (SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request.Cookies))
            {
                return RedirectToAction("Me", "Me");
            }

            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}