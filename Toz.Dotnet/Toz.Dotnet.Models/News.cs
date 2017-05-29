using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class News
    {
        [JsonProperty("id")]  
        public string Id {get; set;}

        [JsonProperty("title")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        [RegularExpression(@"(^(?=.*[a-zA-Z])((\S)+(\s(?!$))?)+)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "InvalidValue")]
        public string Title {get; set;}

        [JsonProperty("published")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? Published  {get; set;}

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? Created  {get; set;}

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime? LastModified {get; set;}

        [JsonProperty("contents")]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]   
        public string Contents {get; set;}

        [JsonIgnore]
        public byte [] Photo {get; set;}

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NewsStatus Type {get; set;}

        [JsonProperty("imageUrl")]
        public string ImageUrl {get; set; }

        }
}