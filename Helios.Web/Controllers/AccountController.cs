using Helios.Storage;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
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
        public IActionResult Login([Bind(Prefix = "credentials.username")] string username, [Bind(Prefix = "credentials.password")] string password, string? _login_remember_me)
        {
            UserData? user = _ctx.UserData.FirstOrDefault(x => x.Email == username && x.Password == password);
            bool rememberMe = _login_remember_me?.ToLower() == "true";

            this.HttpContext.Set<string>("credentials.username", username);
            this.HttpContext.Set<bool>("_login_remember_me", rememberMe);

            if (user == null)
            {
                TempData["Error"] = "Incorrect email or password";

                return View();
            }
            else
            {
                SessionUtil.Login(this.HttpContext, user);

                this.HttpContext.Remove(Constants.CURRENT_AVATAR_ID);

                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = Constants.SESSION_EXPIRY;
                cookieOptions.Path = "/";

                if (rememberMe)
                {
                    var userSessionData = new UserSessionData
                    {
                        SessionId = Guid.NewGuid(),
                        UserId = user.Id,
                        ExpiryDate = Constants.SESSION_EXPIRY
                    };

                    _ctx.UserSessionData.Add(userSessionData);
                    _ctx.SaveChanges();

                    Response.Cookies.Delete(Constants.HELIOS_SESSION);
                    Response.Cookies.Append(Constants.HELIOS_SESSION, userSessionData.SessionId.ToString(), cookieOptions);
                }
                else
                {
                    Response.Cookies.Delete(Constants.HELIOS_SESSION);
                }

                return RedirectToAction("SecurityCheck");
            }
        }

        [HttpGet]
        [Route("/account/logout")]
        public IActionResult Logout()
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
            {
                return RedirectToAction("Index", "Home");
            }

            SessionUtil.Logout(this.HttpContext);

            Response.Cookies.Delete(Constants.HELIOS_SESSION);

            return View();
        }

        [Route("/security_check")]
        public IActionResult SecurityCheck()
        {
            if (!SessionUtil.IsLoggedIn(this._ctx, this.HttpContext, this.Request, this.Response))
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