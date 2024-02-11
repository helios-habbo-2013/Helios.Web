using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Helios.Web.Util;
using Helios.Web.Helpers;
using Helios.Web.Storage;

namespace Helios.Game
{
    public class ViewBagFilter : IActionFilter
    {
        private readonly ValueManager _valueManager;
        private readonly StorageContext _ctx;

        public ViewBagFilter(ValueManager valueManager, StorageContext ctx)
        {
            this._valueManager = valueManager;
            this._ctx = ctx;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
            var controller = context.Controller as Controller;

            if (controller != null)
            {
                controller.ViewBag.SiteName = this._valueManager.GetString("site.name");
                controller.ViewBag.StaticContentUrl = this._valueManager.GetString("site.static.content.url");
                controller.ViewBag.Copyright = "Powered by HavanaWeb by Quackster";
            }

            if (controller != null)
            {
                // If the static content url is empty, default to our own
                if (string.IsNullOrWhiteSpace(controller.ViewBag.StaticContentUrl))
                {
                    HttpRequest request = context.HttpContext.Request;
                    controller.ViewBag.StaticContentUrl = new Uri(request.Scheme + "://" + request.Host.Value).ToString();
                }

                if (context.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
                {
                    controller.ViewBag.Avatar = this._ctx.AvatarData.FirstOrDefault(x => x.Id == context.HttpContext.Get<int>(Constants.CURRENT_AVATAR_ID));
                }

                if (context.HttpContext.Contains(Constants.CURRENT_USER_ID))
                {
                    controller.ViewBag.User = this._ctx.UserData.FirstOrDefault(x => x.Id == context.HttpContext.Get<int>(Constants.CURRENT_USER_ID));
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
