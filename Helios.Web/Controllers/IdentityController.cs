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
    public class IdentityController : Controller
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly StorageContext _ctx;

        public IdentityController(ILogger<IdentityController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/identity/avatars")]
        public IActionResult Avatars()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");

            var avatar = _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID))
                .OrderByDescending(x => x.LastOnline)
                .First();

            var otherAvatars = _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID) && x.Id != avatar.Id)
                .OrderByDescending(x => x.LastOnline)
                .ToList();

            this.ViewBag.Avatar = avatar;
            this.ViewBag.OtherAvatars = otherAvatars;

            return View();
        }

        [Route("/identity/add_avatar")]
        public IActionResult AddAvatar()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");



            string errorType = "";
            string errorMessage = "";
            List<string> suggestions = new List<string>();

            if (HttpContext.Contains("CheckName"))
            {
                string checkName = HttpContext.Get<string>("CheckName");
                RegisterUtil.ValidateNameResponse(ref errorType, ref errorMessage, ref suggestions, checkName, _ctx);
                ViewBag.Name = checkName;
            }
            else
            {
                ViewBag.Name = "";
            }

            ViewBag.ErrorType = errorType;
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.NameSuggestions = suggestions;

            return View();
        }
    }
}