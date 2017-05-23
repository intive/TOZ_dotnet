using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toz.Dotnet.Resources.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Toz.Dotnet.Authorization
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _env;
        private readonly List<string> _controllers;
        private readonly IAuthService _cookieService;

        public CustomAuthorizationMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IHostingEnvironment env, IAuthService cookieService)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _env = env;
            _cookieService = cookieService;

            string[] files = Directory.GetFiles(_env.ContentRootPath + Path.DirectorySeparatorChar + "Controllers");
            _controllers = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                _controllers.Add(Path.GetFileNameWithoutExtension(files[i]).Replace("Controller", ""));
            }
            _controllers.Remove(_appSettings.LoginControllerName);
        }

        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            bool isUrlValid = false;

            if(httpContext.Request.PathBase.HasValue)
            {
                if(httpContext.Request.Path.HasValue)
                {
                    isUrlValid = _controllers.Contains<string>(httpContext.Request.Path.Value.Split('/')[1]);
                }
            }
            else
            {
                isUrlValid = _controllers.Contains<string>(httpContext.Request.Path.Value.Split('/')[1]);
            }

            if (isUrlValid)
            {
                bool isInRole = IsCorrectRoleInToken(httpContext);

                if (!isInRole)
                {
                    await httpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await httpContext.Authentication.ChallengeAsync();
                    _cookieService.RemoveCookie(httpContext, _appSettings.CookieTokenName);
                    return;
                }

                bool authorized = await authorizationService.AuthorizeAsync(
                                        httpContext.User, _appSettings.PolicyName);

                if (!authorized)
                {
                    await httpContext.Authentication.ChallengeAsync();
                    _cookieService.RemoveCookie(httpContext, _appSettings.CookieTokenName);
                    return;
                }
            }

            await _next(httpContext);

        }

        private bool IsCorrectRoleInToken(HttpContext httpContext)
        {
            var token = _cookieService.ReadCookie(httpContext, _appSettings.CookieTokenName);

            if (token != null)
            {
                var jwtPayload = JwtPayload.Base64UrlDeserialize(token.Split('.')[1]);

                var roles = JsonConvert.DeserializeObject<ScopesInToken>(jwtPayload.SerializeToJson());

                for (int i = 0; i < _appSettings.AcceptUserRole.Length; i++)
                {
                    if (roles.Scopes.Contains<string>(_appSettings.AcceptUserRole[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        private class ScopesInToken
        {
            public string[] Scopes { get; set; }
        }
    }
}
