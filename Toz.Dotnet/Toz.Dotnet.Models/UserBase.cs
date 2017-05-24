using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.Validation;

namespace Toz.Dotnet.Models
{
    public class UserBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("phoneNumber")]
        [PhoneNumber(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidPhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("roles", ItemConverterType = typeof(StringEnumConverter))]
        public UserType[] Roles { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("name")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("surname")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string LastName { get; set; }

        public string CombinedName => string.Format("{0} {1}",
            string.IsNullOrEmpty(LastName) ? string.Empty : LastName + ", ",
            string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);

        /// <summary>
        /// Returns a human-readable user description
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}",
                string.IsNullOrEmpty(LastName) ? string.Empty : LastName + ", ",
                string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName);
        }
    }
}
