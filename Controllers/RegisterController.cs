
using Helios.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
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

            if (Request.Form.TryGetValue("beanGender", out var beanGender))
                HttpContext.Set<string>("registerGender", beanGender);

            if (Request.Form.TryGetValue("bean.month", out var beanMonth))
                HttpContext.Set<string>("registerMonth", beanMonth);

            if (Request.Form.TryGetValue("bean.day", out var beanDay))
                HttpContext.Set<string>("registerDay", beanDay);

            if (Request.Form.TryGetValue("bean.year", out var beanYear))
                HttpContext.Set<string>("registerYear", beanYear);

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


            if (!HttpContext.Contains("registerYear") && 
                !HttpContext.Contains("registerMonth") && 
                !HttpContext.Contains("registerDay") && 
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
            if (Request.Form.TryGetValue("bean.username", out var registerName))
                HttpContext.Set<string>("registerName", registerName);

            if (Request.Form.TryGetValue("bean.password", out var registerPassword))
                HttpContext.Set<string>("registerPassword", registerPassword);

            if (Request.Form.TryGetValue("bean.email", out var registerEmail))
                HttpContext.Set<string>("registerEmail", registerEmail);

            if (Request.Form.TryGetValue("bean.referrer", out var registerReferrer))
                HttpContext.Set<string>("registerReferrer", registerReferrer);


            var checkEmpty = new string[] { "bean.username", "bean.email", "bean.password", /*"bean.captcha", */"bean.termsOfServiceSelection" };

            foreach (var field in checkEmpty)
            {
                if (!Request.Form.ContainsKey(field) ||
                    (Request.Form.TryGetValue(field, out var value) && string.IsNullOrEmpty(value)))
                {
                    TempData["Error"] = "fields";
                    return RedirectToAction("Step2");
                }
            }

            /*if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) ||
                (Request.Form.TryGetValue("bean.captcha", out var captcha) && captcha != HttpContext.Get<string>("Captcha")))
            {
                TempData["Error"] = "captcha";
                return RedirectToAction("Step2");
            }*/

            return View("SecurityCheck");
        }

        [HttpPost]
		[Route("/quickregister/complete")]
		public IActionResult Complete()
		{
			/*if (Request.Form.TryGetValue("bean.username", out var registerName))
				HttpContext.Set<string>("registerName", registerName);

			if (Request.Form.TryGetValue("bean.password", out var registerPassword))
				HttpContext.Set<string>("registerPassword", registerPassword);

			if (Request.Form.TryGetValue("bean.email", out var registerEmail))
				HttpContext.Set<string>("registerEmail", registerEmail);

			if (Request.Form.TryGetValue("bean.referrer", out var registerReferrer))
				HttpContext.Set<string>("registerReferrer", registerReferrer);


			var checkEmpty = new string[] { "bean.username", "bean.email", "bean.password", "bean.captcha", "bean.termsOfServiceSelection" };

			foreach (var field in checkEmpty)
			{
				if (!Request.Form.ContainsKey(field) ||
					(Request.Form.TryGetValue(field, out var value) && string.IsNullOrEmpty(value)))
				{
					TempData["Error"] = "fields";
					return RedirectToAction("Step2");
				}
			}

			if (string.IsNullOrEmpty(HttpContext.Get<string>("Captcha")) ||
				(Request.Form.TryGetValue("bean.captcha", out var captcha) && captcha != HttpContext.Get<string>("Captcha")))
			{
				TempData["Error"] = "captcha";
				return RedirectToAction("Step2");
			}
			*/
			return RedirectToAction("Index", "Home");
		}
    }
}