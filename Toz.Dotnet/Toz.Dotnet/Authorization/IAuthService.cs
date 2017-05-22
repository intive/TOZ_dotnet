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
        string ReadCookie(HttpContext httpContext, string key);
        void RemoveCookie(HttpContext httpContext, string key);

        bool IsAuth { get; }
        void SetIsAuth(bool value);
    }
}
