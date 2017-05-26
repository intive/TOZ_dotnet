namespace Toz.Dotnet.Models.ViewModels
{
    public class UserViewModel
    {
        public User TheUser { get; set; }
        public string ReadableName => $"{TheUser.FirstName} {TheUser.LastName}";
    }
}
