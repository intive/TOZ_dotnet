using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Authorization
{
    public class AuthService : IAuthService
    {
        private IDataProtector _protector;
        private AppSettings _appSettings;

        public bool IsAuth { get; private set; }

        public AuthService(IDataProtectionProvider provider, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector(_appSettings.DataProtectorName);
        }

        public void AddToCookie(HttpContext httpContext, string key, string value, CookieOptions cookieOptions)
        {
            httpContext.Response.Cookies.Append(key, _protector.Protect(value), cookieOptions);
        }

        public string ReadCookie(HttpContext httpContext, string key)
        {
            string value;
            try
            {
                value = _protector.Unprotect(httpContext.Request.Cookies[key]);
            }
            catch
            {
                value = null;
            }

            return value;
        }

        public void RemoveCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }

        public void SetIsAuth(bool value)
        {
            IsAuth = value;
        }
    }
}
