using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Models
{
    public class UserBase : Login
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("name")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "FirstnameLetters")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [JsonProperty("surname")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        [RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "LastnameLetters")]
        public string LastName { get; set; }
    }
}
