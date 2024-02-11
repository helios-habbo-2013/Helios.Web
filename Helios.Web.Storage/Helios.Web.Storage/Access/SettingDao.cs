using Helios.Web.Storage.Models.Misc;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class SettingDao
    {
        public static void GetSettings(this StorageContext context, out Dictionary<string, string> settings)
        {
            settings = context.SettingsData.ToList().ToDictionary(x => x.Key, x => x.Value);
        }

        public static bool HasSetting(this StorageContext context, string key)
        {
            return context.SettingsData.Count(x => x.Key == key) > 0;
        }

        public static void SaveSetting(this StorageContext context, string key, string value)
        {
            var setting = context.SettingsData.FirstOrDefault(x => x.Key == key);

            if (setting == null)
            {
                setting = new SettingsData()
                {
                    Key = key,
                    Value = value
                };

                context.SettingsData.Add(setting);
            }
            else
            {
                setting.Value = value;
                context.Update(setting);
            }

            context.SaveChanges();
        }
    }
}
