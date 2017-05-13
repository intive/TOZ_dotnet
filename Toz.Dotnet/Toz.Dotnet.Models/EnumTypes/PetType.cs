using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum PetType
    {
        [EnumMember(Value = "UNIDENTIFIED")]
        Unidentified,
        [EnumMember(Value = "DOG")]
        Dog,
        [EnumMember(Value = "CAT")]
        Cat
    };
}