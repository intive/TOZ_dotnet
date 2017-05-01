namespace Toz.Dotnet.Tests.Helpers
{
    public static class RequestUriHelper
    {
        // public static string BaseUri { get; } = "http://dev.patronage2017.intive-projects.com";
        // public static string PetsUri { get; } = "http://dev.patronage2017.intive-projects.com/pets";
        // public static string NewsUri { get; } = "http://dev.patronage2017.intive-projects.com/news";
        // public static string UsersUri { get; } = "http://dev.patronage2017.intive-projects.com/users";
        public static string BaseUri { get; } = "https://intense-badlands-80645.herokuapp.com";
        public static string PetsUri { get; } = "https://intense-badlands-80645.herokuapp.com/pets";
        public static string NewsUri { get; } = "https://intense-badlands-80645.herokuapp.com/news";
        public static string UsersUri { get; } = "https://intense-badlands-80645.herokuapp.com/users";
        public static string OrganizationInfoUri { get; } = "https://intense-badlands-80645.herokuapp.com/organization/info";
        public static string JwtTokenUrl { get; } = "https://intense-badlands-80645.herokuapp.com/tokens/acquire";
    }
}