using System.Text.Json;

namespace Helios.Web.Helpers
{
	public static class SessionExtensions
	{
		public static void Set<T>(this HttpContext httpCtx, string key, T value)
		{
			httpCtx.Session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T? Get<T>(this HttpContext httpCtx, string key)
		{
			var value = httpCtx.Session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<T>(value);
		}

        public static bool Contains(this HttpContext httpCtx, string key)
        {
            return httpCtx.Session.TryGetValue(key, out var _);
        }


    }
}
