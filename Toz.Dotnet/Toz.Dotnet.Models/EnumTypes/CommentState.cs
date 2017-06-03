using System.Runtime.Serialization;

namespace Toz.Dotnet.Models.EnumTypes
{
    public enum CommentState
    {
        [EnumMember(Value = "ACTIVE")]
        Active,

        [EnumMember(Value = "DELETED")]
        Deleted
    };
}
