using Helios.Web.Storage.Models.Catalogue;
using Helios.Web.Helpers;
using Helios.Web.Storage;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly StorageContext _ctx;

        public AccountController(ILogger<AccountController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [HttpPost]
        [Route("/account/submit")]
        public IActionResult Login([Bind(Prefix = "credentials.username")] string username, [Bind(Prefix = "credentials.password")] string password)
        {
            return Ok();
        }
    }
}