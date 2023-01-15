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

        [Route("/captcha.png")]
        public IActionResult Index()
        {
            var text = CaptchaUtil.Instance.RandomWord() + " " + CaptchaUtil.Instance.RandomWord();//CaptchaUtil.Instance.RandomTextSequence(CaptchaUtil.Instance.Random.Next(4, 7)) + " " + CaptchaUtil.Instance.RandomTextSequence(CaptchaUtil.Instance.Random.Next(4, 7));
            HttpContext.Set("CaptchaUtil", text);
			return File(CaptchaUtil.Instance.Generate(text), "image/png");
        }
    }
}