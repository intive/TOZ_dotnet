using System;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models
{
    public class Pet
    {
        public int Id {get; set;}

        [Required(AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z]+$")]
        public string Name {get; set;}

        [Required]
        public PetType Type {get; set;}

        [Required]
        public PetSex Sex {get; set;}

        public byte [] Photo {get; set;}

        [StringLength(300)]
        public string Description {get; set;}

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
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