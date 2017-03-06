

using System;

using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models
{

    public class Pet
    {
        public int Id {get; set;}
 
        public string Name {get; set;}
 
        public PetType Type {get; set;}
 
        public PetSex Sex {get; set;}
 
        public byte [] Photo {get; set;}
 
        public string Description {get; set;}
 
        public string Address {get; set;}
 
        public DateTime AddingTime  {get; set;}
 
        public DateTime LastEditTime {get; set;}

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