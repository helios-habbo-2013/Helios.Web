using Helios.Web.Storage.Models.Catalogue;
using Helios.Web.Helpers;
using Helios.Web.Storage;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Helios.Web.Storage.Models.User;
using Helios.Web.Storage.Access;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ILogger<CharacterController> _logger;
        private readonly StorageContext _ctx;

        public CharacterController(ILogger<CharacterController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/character/test")]
        public IActionResult Test()
        {
            return View();
        }
    }
}