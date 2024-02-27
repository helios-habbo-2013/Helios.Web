using Helios.Web.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly StorageContext _ctx;

        public ErrorController(ILogger<ErrorController> logger, StorageContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        [Route("/error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int? statusCode)
        {
            this.ViewBag.ErrorStatus = statusCode;

            string page = "error";

            this.ViewBag.Page = page;
            this.ViewBag.Header = page;

            var pagesHablets = _ctx.PagesHabletData
                .Where(x => x.Page == page)
                .OrderBy(x => x.OrderId)
                .Select(x => Tuple.Create(x.Widget, x.Column))
                .ToList();

            this.ViewBag.Hablets = pagesHablets;
            this.ViewBag.Entities = _ctx;

            return View("NotFound");
        }
    }
}