using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Toz.Dotnet.Authorization
{
    public interface IAuthService
    {
        void AddToCookie(HttpContext httpContext, string key, string value, CookieOptions cookieOptions);
        string ReadCookie(HttpContext httpContext, string key, bool encrypt = false);
        void RemoveCookie(HttpContext httpContext, string key);
        string EncryptValue(string value);
        string DecryptValue(string value);

        bool IsAuth { get; }
        void SetIsAuth(bool value);
    }
}
