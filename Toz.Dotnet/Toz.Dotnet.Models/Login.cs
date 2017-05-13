using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class Login
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}