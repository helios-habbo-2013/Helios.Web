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

            HttpContext.Remove(Constants.IDENTIIY_NAME);
            HttpContext.Remove(Constants.IDENTIIY_SELECTED_FIGURE);
            HttpContext.Remove(Constants.IDENTIIY_SELECTED_GENDER);

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

            if (HttpContext.Contains(Constants.IDENTIIY_NAME))
            {
                string? checkName = HttpContext.Get<string>(Constants.IDENTIIY_NAME);

                if (checkName != null)
                {
                    RegisterUtil.ValidateNameResponse(ref errorType, ref errorMessage, ref suggestions, checkName, _ctx);
                    ViewBag.Name = checkName;
                }
            }
            else
            {
                ViewBag.Name = "";
            }

            if (!HttpContext.Contains(Constants.IDENTIIY_SELECTED_FIGURE) &&
                !HttpContext.Contains(Constants.IDENTIIY_SELECTED_GENDER))
            {
                var figure = "hd-180-1.hr-100-61.ch-210-66.lg-270-82.sh-290-80";
                var gender = "M";

                HttpContext.Set<string>(Constants.IDENTIIY_SELECTED_FIGURE, figure);
                HttpContext.Set<string>(Constants.IDENTIIY_SELECTED_GENDER, gender);

                ViewBag.Figure = figure;
                ViewBag.Gender = gender;
            }

            ViewBag.ErrorType = errorType;
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.NameSuggestions = suggestions;

            return View();
        }

        [HttpPost]
        [Route("/identity/add_avatar_add")]
        public IActionResult AddAvatarAdd(string? avatarName)
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");

            HttpContext.Set<string>(Constants.IDENTIIY_NAME, avatarName ?? string.Empty);

            if (RegisterUtil.ValidateNameCode(avatarName ?? "", _ctx) > 0)
                return RedirectToAction("AddAvatar");
            else
            {
                var avatarData = new AvatarData
                {
                    Name = avatarName,
                    Figure = HttpContext.Get<string>(Constants.IDENTIIY_SELECTED_FIGURE),
                    Sex = HttpContext.Get<string>(Constants.IDENTIIY_SELECTED_GENDER)?.ToUpper() ?? "M",
                    UserId = HttpContext.Get<int>(Constants.CURRENT_USER_ID)
                };

                _ctx.AvatarData.Add(avatarData);
                _ctx.SaveChanges();

                return RedirectToAction("Avatars");
            }
        }

        [Route("/identity/settings")]
        public IActionResult Settings()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Route("/identity/email")]
        public IActionResult SettingsEmail()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Route("/identity/password")]
        public IActionResult SettingsPassword()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}