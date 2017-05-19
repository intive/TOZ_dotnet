namespace Toz.Dotnet.Resources.Configuration
{
    public class AppSettings
    {
        public string[] AcceptPhotoTypes { get; set; }

        public string BackendBaseUrl { get; set;}
        public string BackendPetsUrl { get; set; }
        public string BackendNewsUrl { get; set; }
        public string BackendUsersUrl { get; set; }
		public string BackendScheduleUrl { get; set; }
        public string BackendOrganizationInfoUrl { get; set; }
        public string BackendJwtUrl { get; set; }
        public string BackendProposalsUrl { get; set; }
    }
}