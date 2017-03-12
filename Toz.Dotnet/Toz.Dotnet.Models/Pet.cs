using System;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Resources;

namespace Toz.Dotnet.Models
{
    public class Pet
    {
        public int Id {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "EmptyField")]
        [StringLength(30, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "NameLetters")]
        public string Name {get; set;}

        [RegularExpression("^(Cat|Dog)$", ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "TypeUndefined")]
        public PetType Type {get; set;}

        public PetSex Sex {get; set;}

        public byte [] Photo {get; set;}

        [StringLength(300, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
        public string Description {get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.NewPetDataValidation),
                  ErrorMessageResourceName = "MaxLength")]
        public string Address {get; set;}

        public DateTime AddingTime  {get; set;}

        public DateTime LastEditTime {get; set;}

        public Pet() { }

        public Pet(int id, string name, PetType type, PetSex sex, byte [] photo, string description, string address,
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