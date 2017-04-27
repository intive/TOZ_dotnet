using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.JsonConventers;
using Toz.Dotnet.Resources.CustomValidationAttributes;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class Contact
    {
        [JsonProperty("email")]
        [EmailAddress]
        public string Email { get; set; }

        [JsonProperty("phone")]
        [PhoneNumber]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        [PhoneNumber]
        public string Fax { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }
}
