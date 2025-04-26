using EmailValidation;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

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
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            HttpContext.Remove(Constants.IDENTIIY_NAME);
            HttpContext.Remove(Constants.IDENTIIY_SELECTED_FIGURE);
            HttpContext.Remove(Constants.IDENTIIY_SELECTED_GENDER);

            var avatar = _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID))
                .OrderByDescending(x => x.LastOnline)
                .FirstOrDefault();

            var otherAvatars = avatar != null ? _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID) && x.Id != avatar.Id)
                .OrderByDescending(x => x.LastOnline)
                .ToList() : new List<AvatarData>();

            this.ViewBag.Avatar = avatar;
            this.ViewBag.OtherAvatars = otherAvatars;

            return View();
        }

        [Route("/identity/add_avatar")]
        public IActionResult AddAvatar()
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
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

        [HttpGet]
        [Route("/identity/useOrCreateAvatar/{avatarId}")]
        public IActionResult UseOrCreateAvatar(int avatarId)
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            var avatarList = _ctx.AvatarData.Where(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID))
                .OrderByDescending(x => x.LastOnline)
                .ToList();

            if (avatarList.Any(x => x.UserId == this.HttpContext.Get<int>(Constants.CURRENT_USER_ID) && x.Id == avatarId))
                this.HttpContext.Set<int>(Constants.CURRENT_AVATAR_ID, avatarId);

            return RedirectToAction("Me", "Me");
        }

        [HttpPost]
        [Route("/identity/add_avatar_add")]
        public IActionResult AddAvatarAdd(string? avatarName)
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
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
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        [Route("/identity/email")]
        public IActionResult SettingsEmail()
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [Route("/identity/email")]
        public IActionResult UpdateSettingsEmail(string password, string email, string captcha, string directMail)
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            UserData? user = ViewBag.User as UserData;

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(captcha))
            {

                ViewBag.AlertMessage = "Please enter all fields";
                ViewBag.AlertColour = "red";
            }
            else if (user.Password != password)
            {
                ViewBag.AlertMessage = "Your current password is invalid";
                ViewBag.AlertColour = "red";
            }
            else if (!EmailValidator.Validate(email))
            {
                ViewBag.AlertMessage = "The email you entered is invalid";
                ViewBag.AlertColour = "red";
            }
            else if (_ctx.UserData.Any(x => x.Email.ToLower() == email.ToLower()))
            {
                ViewBag.AlertMessage = "This email is already being used by a different account";
                ViewBag.AlertColour = "red";
            }
            else if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) || captcha != HttpContext.Get<string>("Captcha"))
            {
                ViewBag.AlertMessage = "The security code was invalid, please try again.";
                ViewBag.AlertColour = "red";
            }
            else
            {
                ViewBag.AlertMessage = "Your email has been changed successfully.";
                ViewBag.AlertColour = "green";

                user.Email = email;
                user.DirectEmail = directMail == "on" || directMail == "true";

                _ctx.UserData.Update(user);
                _ctx.SaveChanges();
            }


            return View("SettingsEmail");
        }

        [HttpGet]
        [Route("/identity/password")]
        public IActionResult SettingsPassword()
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [Route("/identity/password")]
        public IActionResult UpdateSettingsPassword(string currentpassword, string newpassword, string newpasswordconfirm, string captcha, string directMail)
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
                return RedirectToAction("Index", "Home");

            UserData? user = ViewBag.User as UserData;

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(currentpassword) ||
                string.IsNullOrEmpty(newpassword) ||
                string.IsNullOrEmpty(newpasswordconfirm) ||
                string.IsNullOrEmpty(captcha))
            {

                ViewBag.AlertMessage = "Please enter all fields";
                ViewBag.AlertColour = "red";
            }
            else if (user.Password != currentpassword)
            {
                ViewBag.AlertMessage = "Your current password is invalid";
                ViewBag.AlertColour = "red";
            }
            else if (newpassword.Length < 6)
            {
                ViewBag.AlertMessage = "Password is too short, 6 characters minimum";
                ViewBag.AlertColour = "red";
            }
            else if (newpassword != newpasswordconfirm)
            {
                ViewBag.AlertMessage = "The passwords don't match";
                ViewBag.AlertColour = "red";
            }
            else if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) || captcha != HttpContext.Get<string>("Captcha"))
            {
                ViewBag.AlertMessage = "The security code was invalid, please try again.";
                ViewBag.AlertColour = "red";
            }
            else
            {
                ViewBag.AlertMessage = "Your password has been changed successfully. You will need to login again.";
                ViewBag.AlertColour = "green";

                user.Password = newpassword;

                _ctx.UserData.Update(user);
                _ctx.SaveChanges();

                SessionUtil.Logout(this.HttpContext);
            }

            return View("SettingsPassword");
        }
    }
}