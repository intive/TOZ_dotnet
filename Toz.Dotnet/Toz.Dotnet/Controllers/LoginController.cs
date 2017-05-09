using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Toz.Dotnet.Controllers
{
    public class LoginController : Controller
    {
        private IBackendErrorsService _backendErrorsService;
        private readonly IAuthService _authService; // TEMPORARY

        public LoginController(IAuthService authService, IBackendErrorsService backendErrorsService)
        {
            _authService = authService; // TEMPORARY
            _backendErrorsService = backendErrorsService;
        }

        public async Task<IActionResult> Index()
        {
            // --> TEMPORARY
            for(int i = 0; i < 5; i++)
            {
                await _authService.SignIn(new Models.Login() { Email = $"TOZ_user{i}.email@gmail.com", Password = $"TOZ_name_{i}" });
                if(_authService.IsAuth)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _backendErrorsService.UpdateModelState(ModelState);
                }
            }
            //ViewData["Auth"] = "Nie zalogowano!!!";
            return View();
            // <--
        }
    }
}
