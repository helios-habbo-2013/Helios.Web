using Helios.Storage;
using Helios.Web.Helpers;
using Helios.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Helios.Game
{
    public class GlobalControllerFilter : IActionFilter
    {
        private readonly ValueManager _valueManager;
        private readonly PermissionsManager _permissionsManager;
        private readonly StorageContext _ctx;

        public GlobalControllerFilter(ValueManager valueManager, StorageContext ctx, PermissionsManager permissionsManager)
        {
            this._valueManager = valueManager;
            this._ctx = ctx;
            this._permissionsManager = permissionsManager;
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
                controller.ViewBag.StorageContext = _ctx;
            }

            // var t = _ctx.HousekeepingNotes.Include(x => x.AvatarData).ToList();

            if (controller != null)
            {
                controller.ViewBag.LoggedIn = false;

                // If the static content url is empty, default to our own
                if (string.IsNullOrWhiteSpace(controller.ViewBag.StaticContentUrl))
                {
                    HttpRequest request = context.HttpContext.Request;
                    controller.ViewBag.StaticContentUrl = new Uri(request.Scheme + "://" + request.Host.Value).ToString();
                }

                if (context.HttpContext.Contains(Constants.CURRENT_AVATAR_ID))
                {
                    controller.ViewBag.Avatar = this._ctx.AvatarData.FirstOrDefault(x => x.Id == context.HttpContext.Get<int>(Constants.CURRENT_AVATAR_ID));
                    controller.ViewBag.PermissionGroup = this._permissionsManager[controller.ViewBag.Avatar?.Rank ?? 0];
                    controller.ViewBag.LoggedIn = true;
                }
                else
                {
                    controller.ViewBag.Avatar = null;
                    controller.ViewBag.PermissionGroup = this._permissionsManager[0];
                }

                if (context.HttpContext.Contains(Constants.CURRENT_USER_ID))
                {
                    controller.ViewBag.User = this._ctx.UserData.FirstOrDefault(x => x.Id == context.HttpContext.Get<int>(Constants.CURRENT_USER_ID));
                }
                else
                {
                    controller.ViewBag.User = null;
                }

                controller.ViewBag.Permissions = _permissionsManager;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;


            // do something after the action executes
        }
    }
}
