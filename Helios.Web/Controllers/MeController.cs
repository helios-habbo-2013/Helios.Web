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
            ViewBag.Error = null;//= "no_password";
            return View("Index");
        }
    }
}