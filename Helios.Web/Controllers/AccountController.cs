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
        public IActionResult Login([Bind(Prefix = "credentials.username")] string username, [Bind(Prefix = "credentials.password")] string password, string _login_remember_me)
        {
            UserData? user = _ctx.UserData.FirstOrDefault(x => x.Email == username && x.Password == password);

            if (user == null)
            {
                this.HttpContext.Set<string>("credentials.username", username);
                this.HttpContext.Set<string>("_login_remember_me", _login_remember_me);

                return View();
            }
            else
            {
                SessionUtil.Login(this.HttpContext, user);

                return RedirectToAction("SecurityCheck");
            }
        }

        [HttpPost]
        [Route("/account/logout")]
        public IActionResult Logout()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                return RedirectToAction("Index", "Home");
            }

            SessionUtil.Logout(this.HttpContext, ViewBag.User as UserData ?? new UserData());

            return View();
        }

        [Route("/security_check")]
        public IActionResult SecurityCheck()
        {
            if (!this.HttpContext.Get<bool>(Constants.LOGGED_IN))
            {
                this.ViewBag.RedirectPath = "index";

                return View();
            }

            if (this.HttpContext.Contains(Constants.CURRENT_USER_ID) &&
                !this.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
            {
                // return RedirectToAction("Select", "Identity");
                this.ViewBag.RedirectPath = "identity/avatars";

                return View();
            }

            this.ViewBag.RedirectPath = "me";

            return View();
            // return RedirectToAction("Me", "Me");
        }
    }
}