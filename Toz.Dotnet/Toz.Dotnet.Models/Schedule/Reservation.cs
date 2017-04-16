using System;
using Newtonsoft.Json;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class Reservation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime Date { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("ownerForename")]
        public string OwnerForename { get; set; }

        [JsonProperty("ownerSurname")]
        public string OwnerSurname { get; set; }
        
        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime Created { get; set; }

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime LastModified { get; set; }        
    }
}