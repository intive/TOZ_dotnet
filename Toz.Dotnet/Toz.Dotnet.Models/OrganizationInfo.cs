using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.JsonConventers;
using Toz.Dotnet.Models.OrganizationSubtypes;

namespace Toz.Dotnet.Models
{
    public class OrganizationInfo
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
