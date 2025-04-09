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
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
            {
                return RedirectToAction("Index", "Home");
            }

            if (this.HttpContext.Contains(Constants.CURRENT_USER_ID) &&
                !this.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
            {
                return RedirectToAction("Avatars", "Identity");
            }

            var ssoTicket = "SSO-" + Guid.NewGuid().ToString().ToLower();

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