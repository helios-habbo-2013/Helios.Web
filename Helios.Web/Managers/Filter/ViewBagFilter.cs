using Helios.Web.Storage;
using Helios.Web.Storage.Access;
using Helios.Web.Util.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Helios.Game
{
    public class ViewBagFilter : IActionFilter
    {
        private readonly ValueManager _valueManager;

        public ViewBagFilter(ValueManager valueManager)
        {
            this._valueManager = valueManager;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
            var controller = context.Controller as Controller;

            if (controller != null)
            {
                controller.ViewBag.SiteName = this._valueManager.GetString("site.name");
                controller.ViewBag.StaticContentUrl = this._valueManager.GetString("site.static.content.url");
            }

            if (controller != null)
            {
                // If the static content url is empty, default to our own
                if (string.IsNullOrWhiteSpace(controller.ViewBag.StaticContentUrl))
                {
                    HttpRequest request = context.HttpContext.Request;
                    controller.ViewBag.StaticContentUrl = new Uri(request.Scheme + "://" + request.Host.Value).ToString();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
