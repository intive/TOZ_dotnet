using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class PetsStatus
    {
        [JsonProperty("id")]
        public string Id {get; set;}

        [JsonProperty("public")]
        public bool IsPublic { get; set; }

        [JsonProperty("name")]     
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]     
        public string Name {get; set;}

        [JsonProperty("rgb")]
        [StringLength(7, MinimumLength = 7, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string RGB { get; set; }
    }
}