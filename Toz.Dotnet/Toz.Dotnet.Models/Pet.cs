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
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]     
        public string Name {get; set;}

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [RegularExpression("^(Cat|Dog)$", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "PetTypeUndefined")]
        public PetType Type {get; set;}
        
        [JsonProperty("sex")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PetSex Sex {get; set;}

        [JsonIgnore]
        public byte [] Photo {get; set;}


        [JsonProperty("description")]
        [StringLength(300, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Description {get; set;}


        [JsonProperty("address")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        public string Address {get; set;}

        [JsonProperty("created")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime AddingTime  {get; set;}

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime LastEditTime {get; set;}

        public Pet() { }

        public Pet(string id, string name, PetType type, PetSex sex, byte [] photo, string description, string address,
                    DateTime addingTime, DateTime lastEditTime)
                {
                    Id = id;
                    Name = name;
                    Type = type;
                    Sex = sex;
                    Photo = photo;
                    Description = description;
                    Address = address;
                    AddingTime = addingTime;
                    LastEditTime = lastEditTime;
                 }
        }
}