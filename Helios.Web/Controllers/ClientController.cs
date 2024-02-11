using Avatara;
using Avatara.Figure;
using Helios.Web.Helpers;
using Helios.Web.Storage;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly StorageContext _ctx;

        public ClientController(ILogger<ClientController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/client")]
        public IActionResult Client()
        {
            if (!HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = null;//= "no_password";
            return Ok("test");
        }
    }
}