using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class MeController : Controller
    {
        private readonly ILogger<MeController> _logger;
        private readonly StorageContext _ctx;

        public MeController(ILogger<MeController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/me")]
        public IActionResult Me()
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

            if (ViewBag.Avatar is AvatarData avatar)
            {
                var otherAvatars = avatar != null ? _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID) && x.Id != avatar.Id)
                    .OrderByDescending(x => x.LastOnline)
                    .ToList() : new List<AvatarData>();

                this.ViewBag.OtherAvatars = otherAvatars;
            }

            string page = "me";

            this.ViewBag.Page = page;
            this.ViewBag.Header = page;

            var pagesHabblets = _ctx.PagesHabbletData
                .Where(x => x.Page == page && x.Visible)
                .OrderBy(x => x.OrderId)
                .Select(x => Tuple.Create(x.Widget, x.Column))
                .ToList();

            this.ViewBag.Habblets = pagesHabblets;

            return View("Me_old");
        }

        [Route("/community")]
        public IActionResult Community()
        {
            string page = "community";

            this.ViewBag.Page = page;
            this.ViewBag.Header = page;

            var pagesHabblets = _ctx.PagesHabbletData
                .Where(x => x.Page == page && x.Visible)
                .OrderBy(x => x.OrderId)
                .Select(x => Tuple.Create(x.Widget, x.Column))
                .ToList();

            this.ViewBag.Habblets = pagesHabblets;

            return View();
        }
    }
}