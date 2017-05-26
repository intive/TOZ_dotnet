using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Schedule
{
    public class Config
    {
        [JsonProperty("dayOfWeek")]
        public string DayOfWeek { get; set; }
        [JsonProperty("numberOfPeriods")]
        public int NumberOfPeriods { get; set; }
        [JsonProperty("periods")]
        public Period[] Periods { get; set; }
    }
}
