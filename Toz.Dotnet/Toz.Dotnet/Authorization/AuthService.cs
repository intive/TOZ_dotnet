using System;
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

        public AuthService(IDataProtectionProvider provider, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector(_appSettings.DataProtectorName);
        }

        public void AddToCookie(HttpContext httpContext, string key, string value, CookieOptions cookieOptions)
        {
            httpContext.Response.Cookies.Append(key, EncryptValue(value), cookieOptions);
        }

        public string ReadCookie(HttpContext httpContext, string key, bool encrypted = false)
        {
            string value;
            try
            {
                if (encrypted)
                {
                    value = DecryptValue(httpContext.Request.Cookies[key]);
                }
                else
                {
                    value = httpContext.Request.Cookies[key];
                }
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

        public string EncryptValue(string value)
        {
            return _protector.Protect(value);
        }

        public string DecryptValue(string value)
        {
            return _protector.Unprotect(value);
        }
    }
}
