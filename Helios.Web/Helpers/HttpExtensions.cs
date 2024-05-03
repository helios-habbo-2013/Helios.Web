using Microsoft.AspNetCore.Mvc;

namespace Helios.Web.Helpers
{
    public static class HttpExtensions
    {
		public static string AbsolutePath(this IUrlHelper url, string contentPath)
		{
			HttpRequest request = url.ActionContext.HttpContext.Request;
			return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
		}

        public static string BasePath(this IUrlHelper url)
        {
            HttpRequest request = url.ActionContext.HttpContext.Request;
            return new Uri(request.Scheme + "://" + request.Host.Value).ToString().TrimEnd('/');
        }
    }
}
