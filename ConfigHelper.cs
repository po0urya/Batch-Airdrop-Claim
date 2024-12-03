using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace BatchAirdropClaim
{
    public static class ConfigHelper
    {
        public static AppSettings LoadAppSettings()
        {
            try
            {
                var configFilePath = "appsettings.json";
                if (!File.Exists(configFilePath))
                {
                    throw new FileNotFoundException("appsettings.json file not found.");
                }

                var configJson = File.ReadAllText(configFilePath);
                var appSettings = JsonConvert.DeserializeObject<AppSettings>(configJson);

                if (appSettings == null)
                {
                    throw new Exception("Failed to deserialize appSettings.");
                }

                return appSettings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in LoadAppSettings: {ex.Message}");
                return null;
            }
        }
    }
}