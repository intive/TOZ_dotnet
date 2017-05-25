using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Toz.Dotnet.Models;
using System.Net.Http;
using Toz.Dotnet.Models.Errors;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Routing;

namespace Toz.Dotnet.Controllers
{
    public class AccountController : TozControllerBase<AccountController>
    {
        private readonly IAccountManagementService _accountManagementService;

        public AccountController(IAccountManagementService accountManagementService, IBackendErrorsService backendErrorsService, IStringLocalizer<AccountController> localizer, IOptions<AppSettings> appSettings, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _accountManagementService = accountManagementService;
        }

        public IActionResult SignIn(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new Login());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("Email", "Password")] Login login, string returnUrl = null)
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            AuthService.RemoveCookie(HttpContext, AppSettings.CookieTokenName);
            AuthService.RemoveCookie(HttpContext, AppSettings.CookieRefreshName);

            JwtToken jwtToken;

            jwtToken = await _accountManagementService.SignIn(login);

            if (jwtToken == null)
            {
                var overallError = BackendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    TempData["UnhandledError"] = overallError;
                }
                else
                {
                    TempData["UnhandledError"] = "NotConnect";
                }
                return RedirectToAction("SignIn", new RouteValueDictionary(new { returnUrl = returnUrl }));
            }

            string serializedObject = JsonConvert.SerializeObject(login, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, jwtToken.Name));
            claims.Add(new Claim(ClaimTypes.Surname, jwtToken.Surname));
            claims.Add(new Claim(ClaimTypes.Email, jwtToken.Email));

            for (int i = 0; i < jwtToken.Roles.Length; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, jwtToken.Roles[i]));
            }

            claims.Add(new Claim(ClaimTypes.Hash, AuthService.EncryptValue(serializedObject)));

            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.MaxValue.ToUnixTimeSeconds()),//FromUnixTimeSeconds(jwtToken.ExpirationDateSeconds),
                IsPersistent = true,
                AllowRefresh = false
            });

            // Token
            AuthService.AddToCookie(HttpContext, AppSettings.CookieTokenName, jwtToken.Jwt, new CookieOptions() { Expires = DateTimeOffset.FromUnixTimeSeconds(jwtToken.ExpirationDateSeconds) });

            // Refresh token
            AuthService.AddToCookie(HttpContext, AppSettings.CookieRefreshName, "", new CookieOptions() { Expires = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Convert.ToInt64(TimeSpan.FromMinutes(AppSettings.CookieRefreshTimeInMinutes).TotalSeconds)) });

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public async Task<IActionResult> SignOut()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete(AppSettings.CookieTokenName);
            HttpContext.Response.Cookies.Delete(AppSettings.CookieRefreshName);
            return RedirectToAction("Index", "Home");
        }
    }
}
