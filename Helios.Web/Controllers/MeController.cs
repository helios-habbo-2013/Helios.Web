using Helios.Web.Helpers;
using Helios.Web.Storage;
using Helios.Web.Storage.Models.Avatar;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                return RedirectToAction("Index", "Home");
            }

            if (this.HttpContext.Contains(Constants.CURRENT_USER_ID) &&
                !this.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
            {
                return RedirectToAction("Select", "Identity");
            }

            if (ViewBag.Avatar is AvatarData avatar)
            {
                var otherAvatars = avatar != null ? _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID) && x.Id != avatar.Id)
                    .OrderByDescending(x => x.LastOnline)
                    .ToList() : new List<AvatarData>();

                this.ViewBag.OtherAvatars = otherAvatars;
            }

            var widgets = new Dictionary<string, string>
            {
                { "PersonalInfo", "column1" },
                { "MyCharacters", "column1" },
                { "TopStories", "column2" }
            };

            this.ViewBag.Widgets = widgets;

            return View("Me_old");
        }
    }
}