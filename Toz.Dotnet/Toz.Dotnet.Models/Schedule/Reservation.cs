using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Schedule
{
    public class Reservation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty("ownerSurname")]
        public string OwnerSurname { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("lastModified")]
        public long LastModified { get; set; }
    }
}