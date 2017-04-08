using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class Pet
    {
        [JsonProperty("id")]
        public string Id {get; set;}

        [JsonProperty("name")]     
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]     
        public string Name {get; set;}

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [RegularExpression("^(CAT|DOG)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "PetTypeUndefined")]
        public PetType Type {get; set;}
        
        [JsonProperty("sex")]
        [JsonConverter(typeof(StringEnumConverter))]
        [RegularExpression("^(MALE|FEMALE)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "PetSexUndefined")]
        public PetSex Sex {get; set;}

        [JsonIgnore]
        public byte [] Photo {get; set;}

        [JsonProperty("description")]
        [StringLength(120, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Description {get; set;}

        [JsonProperty("address")]
        [StringLength(35, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Address {get; set;}

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? AddingTime  {get; set;}

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? LastEditTime {get; set;}

        [JsonProperty("imageUrl")]
        public string ImageUrl {get; set;}
    }
}