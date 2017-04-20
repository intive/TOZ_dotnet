using System;
using Newtonsoft.Json;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models.Schedule
{
    public class Reservation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime Created { get; set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime Date { get; set; }

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime LastModified { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("modificationAuthorId")]
        public string ModificationAuthorId { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }
    }
}