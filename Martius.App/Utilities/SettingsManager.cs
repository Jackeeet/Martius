using System.IO;
using Newtonsoft.Json;

namespace Martius.App
{
    internal static class SettingsManager
    {
        private static readonly string _filePath = @"userSettings.json";

        private static readonly AppSettings _defaultSettings = new AppSettings()
        {
            DiscountPercentage = new decimal(5.0),
            MinLeaseCount = 5,
            MinLeaseMonths = 1,
            UserDatabasePath = ""
        };

        internal static AppSettings GetUserSettings()
        {
            AppSettings settings;

            try
            {
                using (var file = File.OpenText(_filePath))
                {
                    var serializer = new JsonSerializer();
                    settings = (AppSettings) serializer.Deserialize(file, typeof(AppSettings));
                }
            }
            catch (FileNotFoundException)
            {
                settings = SetDefaultSettings();
            }

            return settings;
        }

        internal static void SetUserSettings(AppSettings settings)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(settings));
        }

        internal static AppSettings SetDefaultSettings()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_defaultSettings));
            return _defaultSettings;
        }
    }
}