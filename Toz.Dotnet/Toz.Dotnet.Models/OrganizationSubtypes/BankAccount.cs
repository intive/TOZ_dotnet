using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Models.CustomValidationAttributes;

namespace Toz.Dotnet.Models.OrganizationSubtypes
{
    public class BankAccount
    {
        [JsonProperty("number")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [BankAccountNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "BankAccountValidationMessage")]       
        public string Number { get; set; }

        [JsonProperty("bankName")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\.\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?[\.]?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        public string BankName { get; set; }
    }
}
