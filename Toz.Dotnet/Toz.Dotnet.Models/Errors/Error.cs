using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Errors
{
    public class Error
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("rejectedValue")]
        public string RejectedValue { get; set; }
    }
}
