using System.Text.Json;

namespace Helios.Web.Helpers
{
	public static class DateTimeExtensions
	{
        public static string TimeAgo(this DateTime fromTime, DateTime? toTime = null, bool includeSeconds = false)
        {
            if (!toTime.HasValue)
                toTime = DateTime.Now;

            TimeSpan timeDifference = toTime.Value - fromTime;
            double totalMinutes = timeDifference.TotalMinutes;
            int totalSeconds = (int)timeDifference.TotalSeconds;

            if (totalMinutes < 1)
            {
                if (includeSeconds)
                {
                    if (totalSeconds < 5) return "just now";
                    if (totalSeconds < 10) return "less than 10 seconds ago";
                    if (totalSeconds < 20) return "less than 20 seconds ago";
                    if (totalSeconds < 40) return "half a minute ago";
                    if (totalSeconds < 60) return "less than a minute ago";
                    return $"{totalSeconds} seconds ago";
                }
                return "less than a minute ago";
            }
            if (totalMinutes < 2) return "a minute ago";
            if (totalMinutes < 45) return $"{(int)totalMinutes} minutes ago";
            if (totalMinutes < 90) return "about an hour ago";
            if (totalMinutes < 1440) return $"about {(int)(totalMinutes / 60)} hours ago";
            if (totalMinutes < 2880) return "a day ago";
            if (totalMinutes < 43200) return $"{(int)(totalMinutes / 1440)} days ago";
            if (totalMinutes < 86400) return "about a month ago";
            if (totalMinutes < 525600) return $"{(int)(totalMinutes / 43200)} months ago";
            if (totalMinutes < 1051200) return "about a year ago";
            return $"over {(int)(totalMinutes / 525600)} years ago";
        }

    }
}
