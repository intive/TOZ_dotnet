using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public abstract class TozControllerBase<T> : Controller where T : Controller
    {
        protected IBackendErrorsService BackendErrorsService { get; }
        protected IStringLocalizer<T> StringLocalizer { get;} 
        protected AppSettings AppSettings { get; }

        protected TozControllerBase(IBackendErrorsService backendErrorsService, IStringLocalizer<T> localizer, IOptions<AppSettings> settings)
        {
            BackendErrorsService = backendErrorsService;
            StringLocalizer = localizer;
            AppSettings = settings.Value;
        }

        protected void CheckUnexpectedErrors()
        {
            var overallError = BackendErrorsService.UpdateModelState(ModelState);
            if (!string.IsNullOrEmpty(overallError))
            {
                ViewData["UnhandledError"] = overallError;
            }
        }
    }
}