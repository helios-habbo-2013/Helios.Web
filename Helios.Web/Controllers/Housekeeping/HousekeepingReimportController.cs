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
        [Route("/housekeeping/reimport/external-flash-texts")]
        public async Task<IActionResult> ReimportExternalFlashTexts()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.edit.external.flash.texts"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.MainCategory = "SystemStatus";
            ViewBag.CurrentPage = "Loader";

            ViewBag.Settings = _valueManager.ClientValues;

            Dictionary<string, string> data = await DownloadAndParseDataAsync(_valueManager.ClientValues["external.flash_texts"]);

            _ctx.ExternalFlashTextsData.ExecuteDelete();
            _ctx.ExternalFlashTextsData.AddRange(data.Select(x => new ExternalFlashTextsData { Key = x.Key, Value = x.Value }));
            _ctx.SaveChanges();

            return RedirectToAction("LoaderContent");
        }

        [Route("/housekeeping/reimport/external-variables")]
        public async Task<IActionResult> ReimportExternalVariables()
        {
            if (!SessionUtil.HasHousekeepingAuth(this.HttpContext))
            {
                return RedirectToAction("Dashboard");
            }

            if (!ViewBag.PermissionGroup.HasPermission("housekeeping.edit.external.variables"))
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

            Dictionary<string, string> data = await DownloadAndParseDataAsync(_valueManager.ClientValues["external.variables"]);

            _ctx.ExternalVariablesData.ExecuteDelete();
            _ctx.ExternalVariablesData.AddRange(data.Select(x => new ExternalVariablesData { Key = x.Key, Value = x.Value }));
            _ctx.SaveChanges();

            return RedirectToAction("LoaderContent");
        }

        static async Task<Dictionary<string, string>> DownloadAndParseDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(url);
                var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, string> result = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    var parts = line.Split('=', 2); // Split into 2 parts only
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();
                        result[key] = value;
                    }
                }

                return result;
            }
        }
    }
}