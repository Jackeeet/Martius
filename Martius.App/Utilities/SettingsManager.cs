using System.IO;
using Newtonsoft.Json;

namespace Martius.App
{
    internal static class SettingsManager
    {
        private static readonly string _filePath = @"userSettings.json";

        internal static AppSettings GetUserSettings()
        {
            AppSettings settings = null;
            using (var file = File.OpenText(_filePath))
            {
                var serializer = new JsonSerializer();
                settings = (AppSettings) serializer.Deserialize(file, typeof(AppSettings));
            }

            return settings;
        }

        internal static void SetUserSettings(AppSettings settings)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(settings));
        }

        internal static AppSettings SetDefaultSettings()
        {
            var defaultSettings = new AppSettings()
            {
                DiscountPercentage = new decimal(5.0),
                MinLeaseCount = 5,
                MinLeaseMonths = 1,
                UserDatabasePath = ""
            };

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(defaultSettings));
            return defaultSettings;
        }
    }
}