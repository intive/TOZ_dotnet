using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models.Organization
{
    public class Contact
    {
        [JsonProperty("email")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"\d{9}(\d{2})?", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidPhoneNumber")]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [RegularExpression(@"\d{9}(\d{2})?", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidFaxNumber")]
        public string Fax { get; set; }

        [JsonProperty("website")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [Url(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidURL")]
        public string Website { get; set; }
    }
}