using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace Toz.Dotnet.Controllers
{
    public class HomeController : Controller
    {
        private IBackendErrorsService _backendErrorsService;
        private IPetsManagementService _petsManagementService;

        public HomeController(IPetsManagementService petsManagementService, IBackendErrorsService backendErrorsService)
        {
            _petsManagementService = petsManagementService;
            _backendErrorsService = backendErrorsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
