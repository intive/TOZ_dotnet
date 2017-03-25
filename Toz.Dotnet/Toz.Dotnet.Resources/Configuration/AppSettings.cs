namespace Toz.Dotnet.Resources.Configuration
{
    public class AppSettings
    {
        public string[] AcceptPhotoTypes { get; set; }

        public string BackendBaseUrl { get; set;}

        public string BackendPetsUrl{ get; set; }

        //TODO: Add when backend API for Users is available
        //public string BackendUsersUrl{ get; set; }
    }
}