using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class JwtToken
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        [JsonProperty("expirationDateSeconds")]
        public long ExpirationDateSeconds { get; set; }

        [JsonProperty("jwt")]
        public string Jwt { get; set; }

    }
}