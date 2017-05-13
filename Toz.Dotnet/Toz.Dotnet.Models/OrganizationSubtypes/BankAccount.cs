using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.JsonConventers;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Models.CustomValidationAttributes;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class BankAccount
    {
        [JsonProperty("number")]
        [BankAccountNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "BankAccountValidationMessage")]
        //[StringLength(26, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "BankAccountLength", MinimumLength = 26)]
        //[RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "OnlyDigits")]
        public string Number { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }
    }
}
