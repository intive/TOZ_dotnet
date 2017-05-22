using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class User : UserBase
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
