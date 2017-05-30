using System;
using System.Collections.Generic;
using System.Text;

namespace Toz.Dotnet.Models.ViewModels
{
    public class CommentViewModel
    {
        public Comment TheComment { get; set; }
        public User TheUser { get; set; }
        public Pet ThePet { get; set; }

        public DateTime Created => TheComment.Created;
        public string UserIdentity => $"{TheUser.FirstName} {TheUser.LastName}";
        public string PetName => ThePet.Name;
        public string Contents => TheComment.Contents;
    }
}
