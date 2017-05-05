using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Models.CustomValidationAttributes;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class BankAccount
    {
        [JsonProperty("number")]
        [BankAccountNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "BankAccountValidationMessage")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string Number { get; set; }

        [JsonProperty("bankName")]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string BankName { get; set; }
    }
}
