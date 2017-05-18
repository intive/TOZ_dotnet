using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum UserType
    {
        [EnumMember(Value = "ANONYMOUS")]
        Anonymous = 0,

        [EnumMember(Value = "VOLUNTEER")]
        Volunteer = 1,

        [EnumMember(Value = "TOZ")]
        Toz = 2,

        [EnumMember(Value = "SA")]
        Sa = 3
    };
}
