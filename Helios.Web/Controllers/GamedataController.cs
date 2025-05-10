using EmailValidation;
using Helios.Storage;
using Helios.Storage.Models.Avatar;
using Helios.Storage.Models.User;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using System.Text;

namespace Helios.Web.Controllers
{
    public class GamedataController : Controller
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly StorageContext _ctx;

        public GamedataController(ILogger<IdentityController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/gamedata/external_variables/{hash}")]
        public IActionResult ExternalVariables(string hash)
        {
            if (hash == "1")
            {
                // Redirect to the same action but with a different hash value
                string newHash = Guid.NewGuid().ToString();
                string? newUrl = Url.Action("ExternalVariables", new { hash = newHash });

                if (newUrl != null)
                {
                    return RedirectPreserveMethod(newUrl);
                }
            }

            var sb = new StringBuilder();

            foreach (var kvp in _ctx.ExternalVariablesData.ToList())
            {
                sb.AppendLine(kvp.Key + "=" + kvp.Value);
            }

            return Ok(sb.ToString());
        }

        [Route("/gamedata/external_flash_texts/{hash}")]
        public IActionResult ExternalFlashTexts(string hash)
        {
            if (hash == "1")
            {
                // Redirect to the same action but with a different hash value
                string newHash = Guid.NewGuid().ToString();
                string? newUrl = Url.Action("ExternalFlashTexts", new { hash = newHash });

                if (newUrl != null)
                {
                    return RedirectPreserveMethod(newUrl);
                }
            }

            var sb = new StringBuilder();

            foreach (var kvp in _ctx.ExternalFlashTextsData.ToList())
            {
                sb.AppendLine(kvp.Key + "=" + kvp.Value);
            }

            return Ok(sb.ToString());
        }

    }
}