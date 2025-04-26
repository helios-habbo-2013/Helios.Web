using Helios.Storage;
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

            var pagesHabblets = _ctx.PagesHabbletData
                .Where(x => x.Page == page && x.Visible)
                .OrderBy(x => x.OrderId)
                .Select(x => Tuple.Create(x.Widget, x.Column))
                .ToList();

            this.ViewBag.Habblets = pagesHabblets;
            this.ViewBag.Entities = _ctx;

            return View("NotFound");
        }
    }
}