using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class Period
    {
        [JsonProperty("periodStart")]
        public string PeriodStart { get; set; }

        [JsonProperty("periodEnd")]
        public string PeriodEnd { get; set; }
    }
}