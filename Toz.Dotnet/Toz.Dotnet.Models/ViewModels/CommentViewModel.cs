using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string PetName { get; set; }
        public CommentState CurrentState { get; set; }

        public string UserIdentity => $"{Comment.AuthorName} {Comment.AuthorSurname}";
    }
}
