using Helios.Web.Helpers;
using Helios.Storage;
using Helios.Storage.Models.User;
using Suggestor;

namespace Helios.Web.Util
{
    public class SessionUtil
    {
        public static void Login(HttpContext httpContext, UserData user)
        {
            httpContext.Set<int>(Constants.CURRENT_USER_ID, user.Id);
            httpContext.Set<bool>(Constants.LOGGED_IN, true);
            httpContext.Remove(Constants.CURRENT_AVATAR_ID);
        }

        public static void Logout(HttpContext httpContext)
        {
            httpContext.Remove(Constants.CURRENT_USER_ID);
            httpContext.Remove(Constants.CURRENT_AVATAR_ID);
            httpContext.Remove(Constants.LOGGED_IN);
        }
    }
}
