using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum HelperCategory
    {
        [EnumMember(Value = "UNIDENTIFIED")]
        Unidentified,
        [EnumMember(Value = "GUARDIAN")]
        Guardian,
        [EnumMember(Value = "TEMPORARY_HOUSE_CAT")]
        TemporaryHouseCat,
        [EnumMember(Value = "TEMPORARY_HOUSE_DOG")]
        TemporaryHouseDog,
        [EnumMember(Value = "TEMPORARY_HOUSE_OTHER")]
        TemporaryHouseOther
    };
}
