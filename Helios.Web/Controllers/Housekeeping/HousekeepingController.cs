using Helios.Game;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Storage.Models.Site;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Helios.Web.Controllers.Housekeeping
{
    public partial class HousekeepingController : Controller
    {
        private readonly ILogger<HousekeepingController> _logger;
        private readonly StorageContext _ctx;
        private readonly PermissionsManager _permissions;
        private readonly ValueManager _valueManager;

        public HousekeepingController(ILogger<HousekeepingController> logger, StorageContext ctx, PermissionsManager permissions, ValueManager valueManager)
        {
            _logger = logger;
            _ctx = ctx;
            _permissions = permissions;
            _valueManager = valueManager;
        }

        [HttpGet]
        [Route("/housekeeping/login")]
        public IActionResult Login()
        {
            if (SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
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
                return RedirectToAction("Dashboard");
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

            return RedirectToAction("Dashboard");
        }


        [Route("/housekeeping/status/staffoverview")]
        public IActionResult StaffOverview()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.staffoverview"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Staff Overview";

            return View("Status/StaffOverview");
        }
    }
}