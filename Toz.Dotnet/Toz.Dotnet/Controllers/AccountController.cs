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
        private IBackendErrorsService _backendErrorsService;
        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;

        public AccountController(IAccountManagementService accountManagementService, IBackendErrorsService backendErrorsService, IStringLocalizer<AccountController> localizer, IOptions<AppSettings> appSettings, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _accountManagementService = accountManagementService;
            _backendErrorsService = backendErrorsService;
            _appSettings = appSettings.Value;
            _authService = authService;
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
            JwtToken jwtToken;

            jwtToken = await _accountManagementService.LogIn(login);

            if(jwtToken == null)
            {
                var overallError = BackendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    TempData["UnhandledError"] = overallError;
                }
                return RedirectToAction("SignIn", new RouteValueDictionary(new { returnUrl = returnUrl }));
            }

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, jwtToken.Name));
            claims.Add(new Claim(ClaimTypes.Surname, jwtToken.Surname));
            claims.Add(new Claim(ClaimTypes.Email, jwtToken.Email));

            for (int i = 0; i < jwtToken.Roles.Length; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, jwtToken.Roles[i]));
            }

            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(jwtToken.ExpirationDateSeconds),
                IsPersistent = true,
                AllowRefresh = false
            });

            _authService.AddToCookie(HttpContext, _appSettings.CookieTokenName, jwtToken.Jwt, new CookieOptions() { Expires = DateTimeOffset.FromUnixTimeSeconds(jwtToken.ExpirationDateSeconds) });
            _authService.SetIsAuth(true);

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
            HttpContext.Response.Cookies.Delete(_appSettings.CookieTokenName);
            return RedirectToAction("Index", "Home");
        }
    }
}
