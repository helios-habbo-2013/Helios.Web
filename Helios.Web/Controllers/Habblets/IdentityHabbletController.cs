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

            HttpContext.Set<bool>("CheckNameOnly", (bool) ViewBag.CheckNameOnly);

            if (ViewBag.CheckNameOnly)
            {
                ViewBag.Name = avatarName;
                HttpContext.Set<string>("CheckName", avatarName);

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
            bool checkNameOnly = HttpContext.Get<bool>("CheckNameOnly");

            if (checkNameOnly)
            {
                string checkName = HttpContext.Get<string>("CheckName");
                ViewBag.Name = checkName;

                if (string.IsNullOrEmpty(checkName))
                {
                    ViewBag.ErrorType = "error";
                    ViewBag.ErrorMessage = "Name is unavaliable";
                }
                else
                {
                    switch (getNameCheckCode(checkName, _ctx))
                    {
                        case 0:
                            {
                                ViewBag.ErrorType = "name_available";
                                break;
                            }
                        case 1:
                            {
                                ViewBag.ErrorType = "error";
                                ViewBag.ErrorMessage = "Name is longer than 16 characters";
                                break;
                            }
                        case 2:
                            {
                                ViewBag.ErrorType = "error";
                                ViewBag.ErrorMessage = "Name must not be shorter than 2 characters";
                                break;
                            }
                        case 4:
                            {
                                var suggestorSettings = SuggestorService.DefaultSettings;
                                suggestorSettings.MaximumWordLength = 16;

                                ViewBag.ErrorType = "already_exists";
                                ViewBag.NameSuggestions = SuggestorService.GetSuggestions(checkName, suggestorSettings, existsCallback: (checkName) =>
                                {
                                    return _ctx.AvatarData.Any(x => x.Name.ToLower() == checkName.ToLower());
                                });
                                break;
                            }
                        case 3:
                            {
                                ViewBag.ErrorType = "error";
                                ViewBag.ErrorType = "Name contains invalid characters";
                                break;
                            }
                    }
                }
            }

            if (ViewBag.ErrorType == "error")
            {
                return View("../Identity/Habblet/NameErrors");
            }

            if (ViewBag.ErrorType == "already_exists")
            {
                return View("../Identity/Habblet/NameSuggestions");
            }

            if (ViewBag.ErrorType == "name_available")
            {
                return View("../Identity/Habblet/NameAvailable");
            }

            return Ok();
        }

        public static int getNameCheckCode(String name, StorageContext _db)
        {
            int nameCheckCode = 0;

            if (_db.AvatarData.Any(x => x.Name == name))
            {
                nameCheckCode = 4;
            }
            else if (name.Length > 16)
            {
                nameCheckCode = 1;
            }
            else if (name.Length < 2)
            {
                nameCheckCode = 2;
            }
            else if (name.Contains(" ") || !hasAllowedCharacters(name.ToLower(), "1234567890qwertyuiopasdfghjklzxcvbnm-+=?!@:.,$") || name.ToUpper().Contains("MOD-"))
            {
                nameCheckCode = 3;
            }

            return nameCheckCode;
        }

        public static bool hasAllowedCharacters(String str, String allowedChars)
        {
            if (str == null)
            {
                return false;
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (allowedChars.Contains(str.ToCharArray()[i]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}