using Helios.Web.Storage;
using Helios.Web.Storage.Access;
using Helios.Web.Util.Extensions;

namespace Helios.Game
{
    public class ValueManager : ILoadable
    {
        #region Fields

        private readonly StorageContext _ctx;
        private Dictionary<string, string>? _clientValues;

        #endregion

        #region Properties

        private Dictionary<string, string> ClientValues
        {
            get { return _clientValues; }
        }

        #endregion

        #region Constructors

        public ValueManager(StorageContext ctx)
        {
            this._ctx = ctx;

            this.Load();
        }

        public void Load()
        {
            _ctx.GetSettings(out _clientValues);

            foreach (var kvp in GetDefaultValues())
            {
                if (!_ctx.HasSetting(kvp.Key))
                {
                    _ctx.SaveSetting(kvp.Key, kvp.Value);
                    _clientValues[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>
        /// Get default configuration values
        /// </summary>
        public Dictionary<string, string> GetDefaultValues()
        {
            var defaultValues = new Dictionary<string, string>();
            
            defaultValues["max.friends.normal"] = "300";
            defaultValues["max.friends.hc"] = "600";
            defaultValues["max.friends.vip"] = "1100";
            defaultValues["max.rooms.allowed"] = "100";
            defaultValues["max.rooms.allowed.subscribed"] = "200";
            defaultValues["timer.speech.bubble"] = "15";
            defaultValues["inventory.items.per.page"] = "500";
            defaultValues["catalogue.subscription.page"] = "63";
            defaultValues["club.gift.interval"] = "1";
            defaultValues["club.gift.interval.type"] = "MONTH";
            defaultValues["site.name"] = "Habbo";
            defaultValues["site.static.content.url"] = "";

            return defaultValues;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get the integer value by key
        /// </summary>
        public int GetInt(string key)
        {
            if (ClientValues.TryGetValue(key, out string value))
                return value.IsNumeric() ? int.Parse(value) : 0;

            return 0;
        }

        /// <summary>
        /// Get the string value by key
        /// </summary>
        public string GetString(string key)
        {
            return ClientValues.TryGetValue(key, out string value) ? value : null;
        }

        #endregion
    }
}
