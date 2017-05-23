

using System;

using MvcApp.Models.EnumTypes;

namespace MvcApp.Models
{

    public class Pet
    {
        public int ID {get; private set;}

        public string Name {get; private set;}

        public PetType Type {get; private set;}

        public PetSex Sex {get; private set;}

        public byte [] Photo {get; private set;}

        public string Description {get; private set;}

        public string Address {get; private set;}

        public DateTime AddingTime  {get; private set;}

        public DateTime LastEditTime {get; private set;}

        public Pet(int id, string name, PetType type, PetSex sex, byte [] photo, string description, string address,
                    DateTime addingTime, DateTime lastEditTime)
                {
                        ID = id;
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