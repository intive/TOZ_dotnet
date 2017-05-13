using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class Login
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmailValidationMessage")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}