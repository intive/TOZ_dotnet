using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.CustomValidationAttributes;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class Contact
    {
        [JsonProperty("email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "OnlyDigits")]
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
