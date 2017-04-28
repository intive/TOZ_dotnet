using Newtonsoft.Json;
using Toz.Dotnet.Models.OrganizationSubtypes;

namespace Toz.Dotnet.Models
{
    public class Organization
    {
        [JsonProperty("name")]
        public string Name { get; set; }
                
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("bankAccount")]
        public BankAccount BankAccount { get; set; }
    }
}
