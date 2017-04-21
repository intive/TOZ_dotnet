using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.JsonConventers;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class BankAccount
    {
        [JsonProperty("number")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "OnlyDigits")]
        public string Number { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }
    }
}
