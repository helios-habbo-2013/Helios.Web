using Helios.Storage;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly StorageContext _ctx;

        public RegisterController(ILogger<RegisterController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/register")]
        public IActionResult Register()
        {
            return RedirectToAction("Start");
        }


        [Route("/quickregister/start")]
        public IActionResult Start()
        {
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];

            return View("Start");
        }

        [HttpPost]
        [Route("/quickregister/age_gate_submit")]
        public IActionResult AgeGateSubmit()
        {
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];

            if (Request.Query.ContainsKey("p") &&
                Request.Query["p"] == "register")
            {

                return RedirectToAction("Start");
            }

            var checkEmpty = new string[] { "bean.gender", "bean.month", "bean.day", "bean.year" };

            foreach (var field in checkEmpty)
            {
                if (!Request.Form.ContainsKey(field) ||
                    (Request.Form.TryGetValue(field, out var value) && string.IsNullOrEmpty(value)))
                {
                    TempData["Error"] = "fields";
                    return RedirectToAction("Start");
                }
            }

            if (Request.Form.TryGetValue("bean.gender", out var beanGender))
                HttpContext.Set<string>("registerGender", beanGender == "male" ? "M" : "F");

            if (Request.Form.TryGetValue("bean.month", out var beanMonth))
                HttpContext.Set<string>("registerMonth", beanMonth.ToString());

            if (Request.Form.TryGetValue("bean.day", out var beanDay))
                HttpContext.Set<string>("registerDay", beanDay.ToString());

            if (Request.Form.TryGetValue("bean.year", out var beanYear))
                HttpContext.Set<string>("registerYear", beanYear.ToString());

            return RedirectToAction("Step2");
        }

        [Route("/quickregister/step2")]
        public IActionResult Step2()
        {
            if (Request.Query.ContainsKey("p") &&
                Request.Query["p"] == "register")
            {
                return RedirectToAction("Start");
            }

            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];


            if (!HttpContext.Contains("registerYear") ||
                !HttpContext.Contains("registerMonth") ||
                !HttpContext.Contains("registerDay") ||
                !HttpContext.Contains("registerGender"))
            {
                TempData["Error"] = "fields";
                return RedirectToAction("Start");
            }

            return View("Step2");
        }

        [HttpPost]
        [Route("/quickregister/security_check")]
        public IActionResult SecurityCheck()
        {
            // if (Request.Form.TryGetValue("bean.username", out var registerName))
            //    HttpContext.Set<string>("registerName", registerName);

            // if (Request.Form.TryGetValue("bean.referrer", out var registerReferrer))
            //    HttpContext.Set<string>("registerReferrer", registerReferrer);


            var checkEmpty = new string[] { /*"bean.username", */"bean.email", "bean.confirmEmail", "bean.password", /*"bean.captcha", */ };

            foreach (var field in checkEmpty)
            {
                if (!Request.Form.ContainsKey(field) ||
                    (Request.Form.TryGetValue(field, out var value) && string.IsNullOrEmpty(value)))
                {
                    TempData["Error"] = "fields";
                    return RedirectToAction("Step2");
                }
            }

            if (!Request.Form.TryGetValue("bean.termsOfServiceSelection", out var termsOfService))
            {
                TempData["Error"] = "terms_of_service";
                return RedirectToAction("Step2");
            }

            if (Request.Form.TryGetValue("bean.confirmEmail", out var email) && email != HttpContext.Get<string>("registerEmail"))
            {
                TempData["Error"] = "email_confirm";
                return RedirectToAction("Step2");
            }


            string registerPassword = Request.Form["registerPassword"].ToString() ?? "";
            string registerEmail = Request.Form["registerEmail"].ToString() ?? "";
            string directMail = Request.Form["bean.marketing"].ToString() ?? "";

            HttpContext.Set<string>("registerPassword", registerPassword.ToString());
            HttpContext.Set<string>("registerEmail", registerEmail.ToString());
            HttpContext.Set<bool>("registerDirectMail", directMail == "true" ? true : false);

            if (_ctx.UserData.Any(x => x.Email == registerEmail))
            {
                TempData["Error"] = "email_exists";
                return RedirectToAction("Step2");
            }

            /*
            if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) ||
                (Request.Form.TryGetValue("bean.captcha", out var captcha) && captcha != HttpContext.Get<string>("Captcha")))
            {
                TempData["Error"] = "captcha";
                return RedirectToAction("Step2");
            }
            */

            ViewBag.Gender = HttpContext.Get<string>("registerGender");

            return RedirectToAction("Captcha");
        }

        [HttpGet]
        [Route("/quickregister/captcha")]
        public IActionResult Captcha()
        {
            if (Request.Query.ContainsKey("p") &&
                Request.Query["p"] == "register")
            {
                return RedirectToAction("Step2");
            }

            if (!HttpContext.Contains("registerEmail") ||
                !HttpContext.Contains("registerPassword"))
            {
                TempData["Error"] = "fields";
                return RedirectToAction("Step2");
            }

            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];

            ViewBag.Gender = HttpContext.Get<string>("registerGender");

            return View("Captcha");
        }

        [Route("/quickregister/refresh_avatars")]
        public IActionResult RefreshAvatars()
        {
            ViewBag.Gender = HttpContext.Get<string>("registerGender");

            return PartialView("ajax/RefreshAvatars");
        }

        [HttpPost]
        [Route("/quickregister/captcha_submit")]
        public IActionResult Complete()
        {
            if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) ||
                (Request.Form.TryGetValue("bean.captcha", out var captcha) && captcha != HttpContext.Get<string>("Captcha")))
            {
                TempData["Error"] = "captcha";
                return RedirectToAction("Captcha");
            }

            if (!Request.Form.TryGetValue("bean.figure", out var figure))
                HttpContext.Set<string>("registerFigure", figure.ToString());

            var registerEmail = HttpContext.Get<string>("registerEmail");
            var registerPassword = HttpContext.Get<string>("registerPassword");
            var registerGender = HttpContext.Get<string>("registerGender");
            var registerFigure = HttpContext.Get<string>("registerFigure");
            var registerMonth = Convert.ToInt32(HttpContext.Get<string>("registerMonth"));
            var registerDay = Convert.ToInt32(HttpContext.Get<string>("registerDay"));
            var registerYear = Convert.ToInt32(HttpContext.Get<string>("registerYear"));

            var userData = new UserData
            {
                Email = registerEmail,
                Password = registerPassword,
                Birthday = $"{registerDay}.{registerMonth}.{registerYear}",
                DirectEmail = HttpContext.Get<bool>("registerDirectMail")
            };

            _ctx.UserData.Add(userData);
            _ctx.SaveChanges();

            /*
            var avatarData = new AvatarData
            {
                Name = GenerateUsername(registerEmail ?? ""),
                Figure = registerFigure,
                Sex = registerGender,
                UserId = userData.Id
            };

            _ctx.AvatarData.Add(avatarData);
            _ctx.SaveChanges();
            */

            HttpContext.Set<int>(Constants.CURRENT_USER_ID, userData.Id);
            // HttpContext.Set<int>(Constants.CURRENT_AVATAR_ID, avatarData.Id);
            HttpContext.Set<bool>(Constants.LOGGED_IN, true);

            return RedirectToAction("Client", "Client");
        }

        public string GenerateUsername(string email)
        {
            // Extract username part from email
            string[] emailParts = email.Split('@');
            string username = emailParts[0];

            // Check if the username already exists, if so, append a number to make it unique
            int suffix = 1;
            string uniqueUsername = username;
            while (IsUsernameTaken(uniqueUsername))
            {
                uniqueUsername = $"{username}{suffix}";
                suffix++;
            }

            return uniqueUsername;
        }

        // Method to check if a username is taken
        public bool IsUsernameTaken(string username)
        {
            return _ctx.AvatarData.Any(x => x.Name == username);
        }
    }
}