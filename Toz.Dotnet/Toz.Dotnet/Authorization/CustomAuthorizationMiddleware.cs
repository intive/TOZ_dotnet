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
using System.Security.Claims;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Authorization
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _env;
        private readonly List<string> _blockedControllers;
        private readonly IAuthService _authService;
        private readonly IAccountManagementService _accountManagementService;

        public CustomAuthorizationMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IHostingEnvironment env, IAuthService cookieService, IAccountManagementService accountManagementService)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _env = env;
            _authService = cookieService;
            _accountManagementService = accountManagementService;

            _blockedControllers = appSettings.Value.BlockedControllers;
        }

        public async Task Invoke(HttpContext httpContext, IAuthorizationService authorizationService)
        {
            // Refresh token
            if (_authService.ReadCookie(httpContext, CookieAuthenticationDefaults.CookiePrefix + CookieAuthenticationDefaults.AuthenticationScheme) != null && _authService.ReadCookie(httpContext, _appSettings.CookieRefreshName, true) == null)
            {
                try
                {
                    var jsonLogin = _authService.DecryptValue(httpContext.User.FindFirst(ClaimTypes.Hash).Value);
                    Login login = JsonConvert.DeserializeObject<Login>(jsonLogin);
                    JwtToken newJwtToken = await _accountManagementService.SignIn(login);
                    if (newJwtToken != null)
                    {
                        _authService.RemoveCookie(httpContext, _appSettings.CookieTokenName);
                        _authService.AddToCookie(httpContext, _appSettings.CookieTokenName, newJwtToken.Jwt, new CookieOptions() { Expires = DateTimeOffset.FromUnixTimeSeconds(newJwtToken.ExpirationDateSeconds) });
                        _authService.AddToCookie(httpContext, _appSettings.CookieRefreshName, "", new CookieOptions() { Expires = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Convert.ToInt64(TimeSpan.FromMinutes(_appSettings.CookieRefreshTimeInMinutes).TotalSeconds)) });
                    }
                }
                catch { };
            }

            bool isUrlValid = false;

            if (httpContext.Request.Path.HasValue)
            {
                isUrlValid = _blockedControllers.Contains<string>(httpContext.Request.Path.Value.Split('/')[1]);
            }

            if (isUrlValid)
            {
                bool isInRole = IsCorrectRoleInToken(httpContext);

                if (!isInRole)
                {
                    await httpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await httpContext.Authentication.ChallengeAsync();
                    _authService.RemoveCookie(httpContext, _appSettings.CookieTokenName);
                    return;
                }

                bool authorized = await authorizationService.AuthorizeAsync(
                    httpContext.User, _appSettings.PolicyName);

                if (!authorized)
                {
                    await httpContext.Authentication.ChallengeAsync();
                    _authService.RemoveCookie(httpContext, _appSettings.CookieTokenName);
                    return;
                }
            }

            await _next(httpContext);

        }

        private bool IsCorrectRoleInToken(HttpContext httpContext)
        {
            try
            {
                var token = _authService.ReadCookie(httpContext, _appSettings.CookieTokenName, true);

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
                }
            }
            catch { };

            return false;
        }

        private class ScopesInToken
        {
            public string[] Scopes { get; set; }
        }
    }
}
