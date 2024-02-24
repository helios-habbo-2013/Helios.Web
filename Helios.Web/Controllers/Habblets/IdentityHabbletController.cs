using Helios.Web.Storage.Models.Catalogue;
using Helios.Web.Helpers;
using Helios.Web.Storage;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Helios.Web.Storage.Models.User;
using Helios.Web.Storage.Access;
using Helios.Web.Storage.Models.Avatar;
using System.Linq;
using Suggestor;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            HttpContext.Set<bool>("CheckNameOnly", (bool)ViewBag.CheckNameOnly);

            if (ViewBag.CheckNameOnly)
            {
                ViewBag.Name = avatarName;
                HttpContext.Set<string>(Constants.IDENTIIY_NAME, avatarName ?? string.Empty);

                return View("../Identity/Habblet/AddAvatar_CheckName");
            }
            else if (ViewBag.CheckFigureOnly)
            {
                ViewBag.Figure = figure;
                ViewBag.Gender = gender;

                HttpContext.Set<string>(Constants.IDENTIIY_SELECTED_FIGURE, figure);
                HttpContext.Set<string>(Constants.IDENTIIY_SELECTED_GENDER, gender);

                return View("../Identity/Habblet/AddAvatar_SelectFigure");
            }
            else
            {
                if (HttpContext.Contains(Constants.IDENTIIY_SELECTED_FIGURE) && 
                    HttpContext.Contains(Constants.IDENTIIY_SELECTED_GENDER))
                {
                    ViewBag.Figure = HttpContext.Get<string>(Constants.IDENTIIY_SELECTED_FIGURE);
                    ViewBag.Gender = HttpContext.Get<string>(Constants.IDENTIIY_SELECTED_GENDER);
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
            bool checkNameOnly = HttpContext.Get<bool>("CheckNameOnly");

            string errorType = "";
            string errorMessage = "";
            List<string> suggestions = new List<string>();


            if (checkNameOnly)
            {
                string checkName = HttpContext.Get<string>(Constants.IDENTIIY_NAME) ?? string.Empty;
                RegisterUtil.ValidateNameResponse(ref errorType, ref errorMessage, ref suggestions, checkName, _ctx);
                ViewBag.Name = checkName;
            }

            ViewBag.ErrorType = errorType;
            ViewBag.ErrorMessage = errorMessage;  
            ViewBag.NameSuggestions = suggestions;

            if (errorType == "error")
                return View("../Identity/Habblet/AddAvatar_NameErrors");

            if (errorType == "already_exists")
                return View("../Identity/Habblet/AddAvatar_NameSuggestions");

            if (errorType == "name_available")
                return View("../Identity/Habblet/AddAvatar_NameAvailable");

            return Ok();
        }
    }
}