using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class Pet
    {
        [JsonProperty("id")]
        public string Id {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "EmptyField")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "NameLetters")]
                  
        [JsonProperty("name")]          
        public string Name {get; set;}

        [JsonProperty("type")]
        [RegularExpression("^(Cat|Dog)$", ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "TypeUndefined")]
        public PetType Type {get; set;}
        
        [JsonProperty("sex")]
        public PetSex Sex {get; set;}

        public byte [] Photo {get; set;}


        [JsonProperty("description")]
        [StringLength(300, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
        public string Description {get; set;}

        [JsonProperty("address")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
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