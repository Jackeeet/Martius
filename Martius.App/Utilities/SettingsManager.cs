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

        internal static void SetDefaultSettings()
        {
            var settings = new AppSettings()
            {
                DiscountPercentage = new decimal(5.0),
                MinLeaseCount = 1,
                MinLeaseMonths = 1,
                UserDatabasePath = ""
            };
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(settings));
        }
    }
}