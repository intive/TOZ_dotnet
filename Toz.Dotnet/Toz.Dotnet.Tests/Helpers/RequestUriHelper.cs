namespace Toz.Dotnet.Tests.Helpers
{
    public static class RequestUriHelper
    {
        public static string BaseUri { get; } = "https://notexistingapi.com";
        public static string PetsUri { get; } = "https://notexistingapi.com/pets";
        public static string NewsUri { get; } = "https://notexistingapi.com/news";
        public static string UsersUri { get; } = "https://notexistingapi.com/users";
        public static string OrganizationInfoUri { get; } = "https://notexistingapi.com/organization";
        public static string JwtTokenUrl { get; } = "https://notexistingapi.com/jwt";
        public static string ProposalsUri { get; } = "https://notexistingapi.com/proposals";
        public static string WrongUrl { get; } = "Thats not a Url!";
    }
}