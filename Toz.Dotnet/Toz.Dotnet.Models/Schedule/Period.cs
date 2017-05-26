using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Schedule
{
    public class Period
    {
        [JsonProperty("periodEnd")]
        public string PeriodEnd { get; set; }
        [JsonProperty("periodStart")]
        public string PeriodStart { get; set; }
    }
}
