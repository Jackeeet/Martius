using Newtonsoft.Json;

namespace Martius.App
{
    [JsonObject]
    public class AppSettings
    {
        [JsonProperty] public decimal DiscountPercentage { get; set; }
        [JsonProperty] public int MinLeaseCount { get; set; }
        [JsonProperty] public string UserDatabasePath { get; set; }
    }
}