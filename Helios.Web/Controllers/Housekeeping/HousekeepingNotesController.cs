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

        [HttpGet]
        [Route("/housekeeping/dashboard")]
        public IActionResult Dashboard()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
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

            return View("Status/Dashboard");
        }

        [HttpPost]
        [Route("/housekeeping/dashboard")]
        public IActionResult AddNote(string title, string content)
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.dashboard"))
            {
                return RedirectToAction("Index", "Home");
            }


            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Notes";

            _ctx.HousekeepingNotes.Add(new HousekeepingNotes { AvatarId = ViewBag.Avatar.Id, Content = content, Title = title });
            _ctx.SaveChanges();

            return View("Status/Dashboard");
        }

        [Route("/housekeeping/dashboard/delete-note/{noteId}")]
        public IActionResult DeleteNote(string noteId)
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.dashboard"))
            {
                return RedirectToAction("Index", "Home");
            }

            _ctx.HousekeepingNotes.Where(x => x.Id.ToString() == noteId).ExecuteDelete();
            _ctx.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}