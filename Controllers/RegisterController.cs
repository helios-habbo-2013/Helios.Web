
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [Route("/quickregister/start")]
        public IActionResult Start()
        {
            ViewBag.Error = null;//= "no_password";
            return View("Start");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}