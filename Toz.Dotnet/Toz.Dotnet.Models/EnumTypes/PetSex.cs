
using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum PetSex
    {
        [EnumMember(Value = "UNKNOWN")]
        Unknown,
        [EnumMember(Value = "MALE")]
        Male,
        [EnumMember(Value = "FEMALE")]
        Female
    };
}