using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models
{
    public class UserBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("firstName")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        [RegularExpression("^([a-zA-Z]+)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "FirstnameLetters")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("lastName")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        [RegularExpression("^([a-zA-Z '-]+)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "LastnameLetters")]
        public string LastName { get; set; }
    }
}
