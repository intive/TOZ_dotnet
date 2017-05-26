using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Schedule
{
    public class Schedule
    {
        [JsonProperty("configs")]
        public Config[] Configs { get; set; }
        [JsonProperty("reservations")]
        public Reservation[] Reservations { get; set; }
    }
}