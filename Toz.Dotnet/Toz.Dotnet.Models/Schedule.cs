using System.Collections.Generic;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class Schedule
    {
        [JsonProperty("configs")]
        public List<Config> Configs { get; set; }

        [JsonProperty("reservations")]
        public List<Reservartion> Reservations { get; set;}
    }
}