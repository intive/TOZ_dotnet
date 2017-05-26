using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public abstract class TozControllerBase<T> : Controller where T : Controller
    {
        protected IBackendErrorsService BackendErrorsService { get; }
        protected IStringLocalizer<T> StringLocalizer { get;} 
        protected AppSettings AppSettings { get; }
        protected IAuthService AuthService { get; }

        protected TozControllerBase(IBackendErrorsService backendErrorsService, IStringLocalizer<T> localizer, IOptions<AppSettings> settings, IAuthService authService)
        {
            BackendErrorsService = backendErrorsService;
            StringLocalizer = localizer;
            AppSettings = settings.Value;
            AuthService = authService;
        }

        protected void CheckUnexpectedErrors()
        {
            var overallError = BackendErrorsService.UpdateModelState(ModelState);
            if (!string.IsNullOrEmpty(overallError))
            {
                ViewData["UnhandledError"] = overallError;
            }
        }

        protected string CurrentCookiesToken => AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true);
    }
}