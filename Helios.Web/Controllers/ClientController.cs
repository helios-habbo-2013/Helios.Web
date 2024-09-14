using Avatara;
using Avatara.Figure;
using Helios.Web.Helpers;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
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
            if (!SessionUtil.IsLoggedIn(_ctx, this.HttpContext, this.Request.Cookies))
            {
                return RedirectToAction("Index", "Home");
            }

            var ssoTicket = Guid.NewGuid().ToString().ToLower();

            if (ViewBag.Avatar is AvatarData avatar)
            {

                _ctx.Add(new AuthenicationTicketData { AvatarId = avatar.Id, ExpireDate = null, Ticket = ssoTicket });
                _ctx.SaveChanges();
            }

            ViewBag.SsoTicket = ssoTicket;

            return View();
        }
    }
}