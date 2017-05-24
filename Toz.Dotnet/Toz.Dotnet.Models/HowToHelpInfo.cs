using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class HowToHelpInfo
    {
        [JsonProperty("modificationDate")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? ModificationTime { get; set; }

        [JsonProperty("howToHelpDescription")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }


    }
}
