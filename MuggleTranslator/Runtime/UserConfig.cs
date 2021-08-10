using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuggleTranslator.Runtime
{
    public class UserConfig
    {
        public static string MainFolder {
            get {
                var mainFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MuggleTranslator");
                Directory.CreateDirectory(mainFolder);
                return mainFolder;
            }
        }

        public bool EnableScreenshotTranlate { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        private UserConfig() { }

        public static UserConfig Current = new UserConfig();
        
        static UserConfig() {
            var configFilePath = Path.Combine(MainFolder, "config.json");
            if (!File.Exists(configFilePath))
                return;

            var userConfigJson = JToken.Parse(File.ReadAllText(configFilePath));

            Current.EnableScreenshotTranlate = userConfigJson.Value<bool>(nameof(EnableScreenshotTranlate));
            Current.ClientId = userConfigJson.Value<string>(nameof(ClientId));
            Current.ClientSecret = userConfigJson.Value<string>(nameof(ClientSecret));
        }

        public static void Update(bool enableScreenshotTranlate, string clientId, string clientSecret)
        {
            Current.EnableScreenshotTranlate = enableScreenshotTranlate;
            Current.ClientId = clientId;
            Current.ClientSecret = clientSecret;

            //
            var userConfig = JsonConvert.SerializeObject(Current);
            var configFilePath = Path.Combine(MainFolder, "config.json");
            File.WriteAllText(configFilePath, userConfig);
        }
    }
}
