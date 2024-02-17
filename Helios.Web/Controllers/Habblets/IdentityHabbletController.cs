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
    public class IdentityHabbletController : Controller
    {
        private readonly ILogger<IdentityHabbletController> _logger;
        private readonly StorageContext _ctx;

        public IdentityHabbletController(ILogger<IdentityHabbletController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/identity/habblet/add")]
        public IActionResult Avatars(bool? checkNameOnly, bool? checkFigureOnly, bool? refreshAvailableFigures, string? avatarName,
            [Bind(Prefix = "bean.figure")] string figure, [Bind(Prefix = "bean.gender")] string gender)
        {
            if (checkNameOnly is bool val)
                ViewBag.CheckNameOnly = val;
            else
                ViewBag.CheckNameOnly = false;

            if (refreshAvailableFigures is bool val2)
                ViewBag.RefreshAvailableFigures = val2;
            else
                ViewBag.RefreshAvailableFigures = false;

            if (checkFigureOnly is bool val3)
                ViewBag.CheckFigureOnly = val3;
            else
                ViewBag.CheckFigureOnly = false;

            if (ViewBag.CheckNameOnly)
            {
                ViewBag.Name = avatarName;
                return View("../Identity/Habblet/CheckName");
            }
            else if (ViewBag.CheckFigureOnly)
            {
                ViewBag.Figure = figure;
                ViewBag.Gender = gender;

                HttpContext.Set<string>("IdentitySelectFigure", figure);
                HttpContext.Set<string>("IdentitySelectGender", gender);

                return View("../Identity/Habblet/SelectFigure");
            }
            else
            {
                if (HttpContext.Contains("IdentitySelectFigure") && HttpContext.Contains("IdentitySelectGender"))
                {
                    ViewBag.Figure = HttpContext.Get<string>("IdentitySelectFigure");
                    ViewBag.Gender = HttpContext.Get<string>("IdentitySelectGender");
                }
                else
                {
                    ViewBag.Figure = null;
                    ViewBag.Gender = null;
                }

                return View("../Identity/Habblet/RandomAvatars");
            }
        }

        [Route("/identity/habblet/add_avatar_messages")]
        public IActionResult Avatars()
        {
            return Ok();//("<div class=\"error-messages-holder\"><h3>Please fix the following problems and resubmit the form.</h3><ul><li><p class=\"error-message\">The name {$user} is already in use</p></li></ul></div>");
        }
    }
}