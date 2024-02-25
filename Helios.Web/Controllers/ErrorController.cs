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
            /*
            var widgets = new List<Tuple<string, string>>();
            */

            var widgets = new List<Tuple<string, string>>
            {
                { Tuple.Create("PageNotFound", "column1") } ,
                { Tuple.Create("WhatWereYouLookingFor", "column2") } ,
            };


            this.ViewBag.Widgets = widgets;
            this.ViewBag.ErrorStatus = statusCode;
            this.ViewBag.Entities = _ctx;

            this.ViewBag.Header = "error";
            this.ViewBag.Page = "error";

            return View("NotFound");
        }
    }
}