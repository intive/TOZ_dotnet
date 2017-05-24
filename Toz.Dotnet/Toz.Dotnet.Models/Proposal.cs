using System;
using Newtonsoft.Json;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class Proposal : UserBase
    {
        [JsonProperty("creationDate")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? CreationTime { get; set; }

        [JsonProperty("read")]
        public bool IsRead { get; set; }
    }
}
