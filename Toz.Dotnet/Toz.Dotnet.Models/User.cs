using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Resources.CustomValidationAttributes;

namespace Toz.Dotnet.Models
{
    public class User : UserBase
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("phoneNumber")]        
        [PhoneNumber]
        public string PhoneNumber {get; set;}
        
		[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("email")]
        [EmailAddress]
        public string Email {get; set;}
        
		[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("purpose")]
        [JsonConverter(typeof(StringEnumConverter))]
        [RegularExpression("^(Administrator|Volunteer|TemporaryHome)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "TypeUndefined")]
        public UserType Purpose {get; set;}

    }
}
