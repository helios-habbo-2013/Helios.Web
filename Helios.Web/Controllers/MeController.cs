using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Helios.Web.Controllers
{
    public class MeController : Controller
    {
        private readonly ILogger<MeController> _logger;

        public MeController(ILogger<MeController> logger)
        {
            _logger = logger;
        }

        [Route("/me")]
        public IActionResult Me()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                return RedirectToAction("Index", "Home");
            }

            if (this.HttpContext.Contains(Constants.CURRENT_USER_ID) && 
                !this.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
            {
                return RedirectToAction("Choose", "Account");
            }

            ViewBag.Error = null; //= "no_password";
            return View("Index");
        }
    }
}