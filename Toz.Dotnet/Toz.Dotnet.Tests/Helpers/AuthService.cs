using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Tests.Helpers
{
    public class AuthService
    {
        public IAuthService AuthHelper { get; set; }

        public AuthService()
        {
            AuthHelper = ServiceProvider.Instance.Resolve<IAuthService>();
            AuthHelper.RequestUri = RequestUriHelper.JwtTokenUrl;
        }

        public async Task<bool> SignIn()
        {
            for (int i = 0; i < 5; i++)
            {
                await AuthHelper.SignIn(new Login()
                {
                    Email = $"TOZ_user{i}.email@gmail.com",
                    Password = $"TOZ_name_{i}"
                });

                if (AuthHelper.IsAuth)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
