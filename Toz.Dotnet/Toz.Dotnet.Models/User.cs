using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Resources.CustomValidationAttributes;

namespace Toz.Dotnet.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id {get; set;}

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("firstName")]   
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]     
        public string FirstName {get; set;}

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("lastName")]        
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]     
        public string LastName {get; set;}
                
        [JsonProperty("phoneNumber")]        
        [PhoneNumber]
        public string PhoneNumber {get; set;}
        
        [JsonProperty("email")]
        [EmailAddress]
        public string Email {get; set;}
        
        [JsonProperty("purpose")]
        [JsonConverter(typeof(StringEnumConverter))]
        [RegularExpression("^(Administrator|Volunteer|TemporaryHome)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "TypeUndefined")]
        public UserType Purpose {get; set;}

    }
}
