using Helios.Web.Helpers;
using Helios.Storage;
using Helios.Storage.Models.User;
using Suggestor;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Ubiety.Dns.Core;

namespace Helios.Web.Util
{
    public class SessionUtil
    {
        public static void Login(HttpContext httpContext, UserData user)
        {
            httpContext.Set<int>(Constants.CURRENT_USER_ID, user.Id);
            httpContext.Set<bool>(Constants.LOGGED_IN, true);
        }

        public static void Logout(HttpContext httpContext)
        {
            httpContext.Remove(Constants.CURRENT_USER_ID);
            httpContext.Remove(Constants.CURRENT_AVATAR_ID);
            httpContext.Remove(Constants.LOGGED_IN);
        }

        public static bool IsLoggedIn(StorageContext ctx, HttpContext httpContext, IRequestCookieCollection cookies)
        {
            bool isLoggedIn = false;

            // Attempt to authenticate using the session cookie
            if (cookies.TryGetValue(Constants.HELIOS_SESSION, out var sessionId))
            {
                // Retrieve user session data from the context
                var userSessionData = ctx.UserSessionData
                    .FirstOrDefault(x => x.SessionId == sessionId);

                // Validate session and retrieve user data if valid
                if (userSessionData != null && userSessionData.ExpiryDate > DateTime.Now)
                {
                    var userData = ctx.UserData
                        .FirstOrDefault(x => x.Id == userSessionData.UserId);

                    if (userData != null)
                    {
                        // Perform login
                        Login(httpContext, userData);
                        isLoggedIn = true;
                    }
                }
            }
            else
            {
                if (httpContext.Get<bool>(Constants.LOGGED_IN))
                {
                    isLoggedIn = true;
                }
            }

            return isLoggedIn;
        }

    }
}
