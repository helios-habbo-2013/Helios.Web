using Helios.Storage;
using Helios.Storage.Access;
using Helios.Web.Util.Extensions;

namespace Helios.Game
{
    public class ValueManager
    {
        #region Fields

        private readonly StorageContext _ctx;
        private Dictionary<string, string> _clientValues;

        #endregion

        #region Properties

        public Dictionary<string, string> ClientValues
        {
            get { return _clientValues; }
        }

        #endregion

        #region Constructors

        public ValueManager(StorageContext ctx)
        {
            this._ctx = ctx;
            this._clientValues = new Dictionary<string, string>();

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
            defaultValues["site.welcome.room"] = "true";
            defaultValues["site.tos"] = "Terms Of Service goes here...";
            defaultValues["site.privacy.policy"] = "Privacy Policy goes here...";

            defaultValues["connection.info.host"] = "localhost";
            defaultValues["connection.info.port"] = "38101";

            defaultValues["external.variables"] = "https://images.h4bbo.net/gamedata/external_variables.txt";
            defaultValues["external.flash.texts"] = "https://images.h4bbo.net/gamedata/external_flash_texts.txt";

            defaultValues["habbo.swf"] = "https://images.h4bbo.net/gordon/RELEASE49-26182-26181-201004270119_de461464ed1e27f56cc890464241d93b/Habbo.swf";

            /*
                                        <td>Connection Info Host</td>
                            <td><input type="text" name="info_host" placeholder="Enter the connection info host" style="width: 50%" value="@(ViewBag.Settings["info.host"])"></td>
                            <td>The server address used for connection information (default: localhost).</td>
                        </tr>
                        <tr>
                            <td>Connection Info Port</td>
                            <td><input type="number" name="info_port" placeholder="Enter the connection info port" style="width: 50%" value="@(ViewBag.Settings["info.port"])"></td>
                            <td>The port number used for connection information (default: 38101).</td>
                        </tr>
                        <tr>
                            <td>External Variables URL</td>
                            <td><input type="text" name="vars_url" placeholder="Enter the external variables URL" style="width: 50%" value="@(ViewBag.Settings["vars.url"])"></td>
                            <td>URL pointing to the external variables file.</td>
                        </tr>
                        <tr>
                            <td>External Texts URL</td>
                            <td><input type="text" name="texts_url" placeholder="Enter the external texts URL" style="width: 50%" value="@(ViewBag.Settings["texts.url"])"></td>
                            <td>URL pointing to the external texts file .</td>
                        </tr>
                        <tr>
                            <td>Movie DCR Path</td>
                            <td><input type="text" name="movie_url" placeholder="Enter the Habbo.swf path" style="width: 50%" value="@(ViewBag.Settings["movie.url"])"></td>*/

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
