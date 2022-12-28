using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class CaptchaController : Controller
    {
        private readonly ILogger<CaptchaController> _logger;

        public CaptchaController(ILogger<CaptchaController> logger)
        {
            _logger = logger;
        }

        [Route("/captcha")]
        public IActionResult Index()
        {
            var text = Captcha.RandomTextSequence(7);
            HttpContext.Set("Captcha", text);
			return File(Captcha.Generate(text), "image/png");
        }
    }
}