using Helios.Storage;
using Helios.Storage.Access;
using Helios.Web.Util.Extensions;

namespace Helios.Game
{
    public class ValueManager : ILoadable
    {
        #region Fields

        private readonly StorageContext _ctx;
        private Dictionary<string, string> _clientValues;

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
            this._clientValues = new Dictionary<string, string>(); 

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
            if (ClientValues.TryGetValue(key, out string? value))
                return value.IsNumeric() ? int.Parse(value) : 0;

            return 0;
        }

        /// <summary>
        /// Get the string value by key
        /// </summary>
        public string GetString(string key)
        {
            return ClientValues.TryGetValue(key, out string? value) ? value : string.Empty;
        }

        #endregion
    }
}
