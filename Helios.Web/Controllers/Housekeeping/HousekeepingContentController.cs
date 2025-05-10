using Avatara;
using Helios.Game;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Storage.Models.Site;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Helios.Web.Util.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Helios.Web.Controllers.Housekeeping
{
    public partial class HousekeepingController : Controller
    {

        [HttpGet]
        [Route("/housekeeping/status/hotel-content")]
        public IActionResult HotelContent()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.hotelcontent"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Hotel Content";

            ViewBag.Settings = _valueManager.ClientValues;

            return View("Status/Content/Hotel");
        }

        [HttpGet]
        [Route("/housekeeping/status/static-content")]
        public IActionResult StaticContent()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.staticcontent"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Static Content";

            ViewBag.Settings = _valueManager.ClientValues;

            return View("Status/Content/Static");
        }

        [HttpGet]
        [Route("/housekeeping/status/loader-content")]
        public IActionResult LoaderContent()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.loadercontent"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (TempData.ContainsKey("AlertType"))
                ViewBag.AlertType = TempData["AlertType"];

            if (TempData.ContainsKey("AlertMessage"))
                ViewBag.AlertMessage = TempData["AlertMessage"];

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Loader";

            ViewBag.Settings = _valueManager.ClientValues;

            return View("Status/Content/Loader");
        }


        [HttpPost]
        [Route("/housekeeping/status/hotel-content")]
        public IActionResult SaveHotelContent(string hotel_name, string welcome_room_enabled, string tos, string privacy_policy)
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.hotelcontent"))
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "Hotel content settings have successfully saved!";


            _ctx.SettingsData.Where(x => x.Key == "site.name")
                .ExecuteUpdate(x =>
                    x.SetProperty(u => u.Value, hotel_name)
                );


            _ctx.SettingsData.Where(x => x.Key == "site.welcome.room")
                .ExecuteUpdate(x =>
                    x.SetProperty(u => u.Value, welcome_room_enabled.IsChecked() ? "true" : "false")
                );


            _ctx.SettingsData.Where(x => x.Key == "site.tos")
                .ExecuteUpdate(x =>
                    x.SetProperty(u => u.Value, tos)
                );


            _ctx.SettingsData.Where(x => x.Key == "site.privacy.policy")
                .ExecuteUpdate(x =>
                    x.SetProperty(u => u.Value, privacy_policy)
                );

            return RedirectToAction("HotelContent");
        }

        [HttpPost]
        [Route("/housekeeping/status/static-content")]
        public IActionResult SaveStaticContent(string static_path)
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.staticcontent"))
            {
                return RedirectToAction("Index", "Home");
            }


            _ctx.SettingsData.Where(x => x.Key == "site.static.content.url")
                .ExecuteUpdate(x =>
                    x.SetProperty(u => u.Value, static_path)
                );

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "Static content settings have successfully saved!";



            return RedirectToAction("StaticContent");
        }

        [HttpPost]
        [Route("/housekeeping/status/loader-content")]
        public IActionResult SaveLoaderSettings(
            string info_host,
            int info_port,
            string vars_url,
            string texts_url,
            string movie_url)
        {

            // Example: Save these settings to your storage, database, or configuration
            var settings = new Dictionary<string, string>
            {
                { "connection.info.host", info_host },
                { "connection.info.port", info_port.ToString() },
                { "external.variables", vars_url },
                { "external.flash.texts", texts_url },
                { "habbo.swf", movie_url }
            };

            // Save each setting
            foreach (var setting in settings)
            {
                _ctx.SettingsData.Where(x => x.Key == setting.Key)
                    .ExecuteUpdate(x =>
                        x.SetProperty(u => u.Value, setting.Value)
                    );
            }

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "Client loader settings have successfully saved!";

            return RedirectToAction("LoaderContent");
        }
    }
}