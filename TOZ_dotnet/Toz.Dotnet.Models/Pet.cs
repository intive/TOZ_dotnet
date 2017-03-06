

using System;

using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models
{

    public class Pet
    {
        private int _id;

        private string _name;

        private PetType _type;

        private PetSex _sex;

        private byte [] _photo;

        private string _description;

        private string _address;

        private DateTime _addingTime;

        private DateTime _lastEditTime;

        //Properties:
        public int Id {get {return _id;}}

        public string Name {get {return _name;}}

        public PetType Type {get {return _type;}}

        public PetSex Sex {get {return _sex;}}

        public byte [] Photo {get {return _photo;}}

        public string Description {get {return _description;}}

        public string Address {get {return _address;}}

        public DateTime AddingTime {get {return _addingTime;}}

        public DateTime LastEditTime { get {return _lastEditTime;}}

        public Pet(int id, string name, PetType type, PetSex sex, byte [] photo, string description, string address,
                    DateTime addingTime, DateTime lastEditTime)
                {
                        _id = id;
                        _name = name;
                        _type = type;
                        _sex = sex;
                        _photo = photo;
                        _description = description;
                        _address = address;
                        _addingTime = addingTime;
                        _lastEditTime = lastEditTime;
                }
    }
}