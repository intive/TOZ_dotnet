using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.CustomValidationAttributes;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models
{
    public class User : UserBase
    {      
        [JsonProperty("phoneNumber")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvaildPhoneNumber")]
        public string PhoneNumber {get; set;}
        	
        [JsonProperty("email")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email {get; set;}
       	
        [JsonProperty("purpose")]
        [JsonConverter(typeof(StringEnumConverter))]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [Range(0,2, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "TypeUndefined")]
        public UserType Purpose {get; set;}
    }
}
