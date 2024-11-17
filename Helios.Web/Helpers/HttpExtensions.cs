using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Helios.Web.Helpers
{
    public static class HttpExtensions
    {
		public static string AbsolutePath(this IUrlHelper url, string contentPath)
		{
/*
            var forwardedHeaders = new Dictionary<string, string>();

            // Accessing the X-Forwarded headers
            foreach (var header in url.ActionContext.HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-Forwarded-"))
                {
                    forwardedHeaders[header.Key] = header.Value;
                }
            }

            // For debugging purposes, printing the X-Forwarded-* headers
            foreach (var header in forwardedHeaders)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }
*/
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
