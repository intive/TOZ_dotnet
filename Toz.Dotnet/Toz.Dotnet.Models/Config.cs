using System.Collections.Generic;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class Config
    {
        [JsonProperty("dayOfWeek")]
        public string DayOfWeek { get; set; }

        [JsonProperty("numberOfPeriods")]
        public int NumberOfPeriods { get; set; }

        [JsonProperty("periods")]
        List<Period> Periods { get; set; }
    }
}