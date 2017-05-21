using System;
using Newtonsoft.Json;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class HowToHelpInfo
    {
        [JsonProperty("modificationDate")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? CreationTime { get; set; }

        [JsonProperty("howToHelpDescription")]
        public string Description { get; set; }


    }
}
