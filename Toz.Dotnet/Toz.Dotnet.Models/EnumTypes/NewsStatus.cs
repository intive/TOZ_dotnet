using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum NewsStatus
    {
        [EnumMember(Value = "RELEASED")]
        Released,
        [EnumMember(Value = "UNRELEASED")]
        Unreleased,
        [EnumMember(Value = "ARCHIVED")]
        Archived
    };
}