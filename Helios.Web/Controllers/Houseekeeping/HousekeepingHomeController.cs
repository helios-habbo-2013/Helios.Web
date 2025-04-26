using Helios.Game;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers.Housekeeping
{
    public partial class HousekeepingController : Controller
    {
        private readonly ILogger<HousekeepingController> _logger;
        private readonly StorageContext _ctx;
        private readonly PermissionsManager _permissions;

        public HousekeepingController(ILogger<HousekeepingController> logger, StorageContext ctx, PermissionsManager permissions)
        {
            _logger = logger;
            _ctx = ctx;
            _permissions = permissions;
        }

        [HttpGet]
        [Route("/housekeeping/login")]
        public IActionResult Login()
        {
            if (SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Housekeeping", "Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping"))
            {
                return RedirectToAction("Home", "Index");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            return View("Login");
        }

        [HttpPost]
        [Route("/housekeeping/login")]
        public IActionResult TryLogin(string email, string password)
        {
            if (SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Housekeeping", "Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping"))
            {
                return RedirectToAction("Index", "Home");
            }

            UserData? user = _ctx.UserData.FirstOrDefault(x => x.Email == email && x.Password == password);
            AvatarData? avatarData = _ctx.AvatarData.FirstOrDefault(x => x.Id == HttpContext.Get<int>(Constants.CURRENT_AVATAR_ID));

            if (user == null || avatarData == null || !_permissions[avatarData.Rank].HasPermission("housekeeping"))
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "You do not have access";

                return RedirectToAction("Login", "Housekeeping");
            }

            this.HttpContext.Set<bool>(Constants.HK_LOGGED_IN, true);
            return RedirectToAction("Dashboard", "Housekeeping");
        }

        [Route("/housekeeping/dashboard")]
        public IActionResult Dashboard()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Housekeeping", "Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.dashboard"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Notes";

            return View("Dashboard");
        }


        [Route("/housekeeping/status/uptime")]
        public IActionResult Uptime()
        {
            if (SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Housekeeping", "Dashboard");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Server Uptime";

            return View("Dashboard");
        }
    }
}