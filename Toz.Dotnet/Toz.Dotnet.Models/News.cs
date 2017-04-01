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
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]    
        public string Title {get; set;}

        [JsonProperty("published")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public DateTime PublishingTime  {get; set;}

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime AddingTime  {get; set;}

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime LastEditTime {get; set;}

        [JsonProperty("contents")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]   
        public string Body {get; set;}

        [JsonIgnore]
        public byte [] Photo {get; set;}

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NewsStatus Status {get; set;}

        [JsonProperty("photoUrl")]
        public string PhotoUrl {get; set; }

        public News() {}
        
        public News(string id, string title, DateTime publishingTime, DateTime addingTime, DateTime lastEditTime,
                    string body, byte[] photo, NewsStatus status)
                {
                    Id = id;
                    Title = title;
                    PublishingTime = publishingTime;
                    AddingTime = addingTime;
                    LastEditTime = lastEditTime;
                    Body = body;
                    Photo = photo;
                    Status = status;
                 }
        }
}